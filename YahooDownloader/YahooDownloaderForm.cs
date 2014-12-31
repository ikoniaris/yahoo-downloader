// 
// YahooDownloaderForm.cs
//  
// Copyright (c) 2015 Peter Cerno
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaasOne.Finance.YahooFinance;
using MySql.Data.MySqlClient;

namespace YahooDownloader
{
    /// <summary>
    /// Form for downloading the financial data from <a href="http://finance.yahoo.com/">Yahoo! Finance</a>.
    /// </summary>
    public partial class YahooDownloaderForm : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public YahooDownloaderForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the clicking on the Start button. Starts the asynchronous download of the financial data.
        /// </summary>
        private async void startButton_Click(object sender, EventArgs e)
        {
            EnableStop();
            var connectionString = string.Format(Culture,
                @"server={0};userid={1};password={2};database={3}",
                hostTextBox.Text, userTextBox.Text, passwordTextBox.Text, databaseTextBox.Text);
            try
            {
                // Open the MySQL connection.
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
                Log("Successfully connected to database: " + _connection.Database);
                using (_cancelSource = new CancellationTokenSource())
                {
                    // Download the financial data.
                    var token = _cancelSource.Token;
                    if (downloadSectorsAndIndustriesCheckBox.Checked)
                        await DownloadSectorsAndIndustries(token);
                    if (downloadCompanyIDsCheckBox.Checked)
                        await DownloadCompanyIDs(token);
                    if (downloadSectorMarketQuotesCheckBox.Checked)
                        await DownloadSectorMarketQuotes(token);
                    if (downloadIndustryMarketQuotesCheckBox.Checked)
                        await DownloadIndustryMarketQuotes(token);
                    if (downloadCompanyMarketQuotesCheckBox.Checked)
                        await DownloadCompanyMarketQuotes(token);
                    if (downloadCompanyProfileCheckBox.Checked ||
                        downloadCompanyQuotesCheckBox.Checked)
                    {
                        _topCompanyIDs = await GetTopCompanyIDs(token);
                        if (downloadMissingTopStocksCheckBox.Checked)
                        {
                            _frozenCompanyProfileIDs = new SortedSet<string>(
                                await GetCompanyIDs("company_profile", token));
                            _frozenCompanyStatisticsIDs = new SortedSet<string>(
                                await GetCompanyIDs("company_valuation_measures", token));
                            _frozenCompanyQuotesIDs = new SortedSet<string>(
                                await GetCompanyIDs("company_quotes", token));
                        }
                        else
                        {
                            _frozenCompanyProfileIDs = new SortedSet<string>();
                            _frozenCompanyStatisticsIDs = new SortedSet<string>();
                            _frozenCompanyQuotesIDs = new SortedSet<string>();
                        }
                        if (_topCompanyIDs == null || _topCompanyIDs.Length == 0)
                        {
                            Log("No Top Stocks to be downloaded!");
                            Log("Make sure that you have downloaded Company IDs and Company Market Quotes.");
                        }
                        else
                        {
                            if (downloadCompanyProfileCheckBox.Checked)
                                await DownloadCompanyProfile(token);
                            if (downloadCompanyQuotesCheckBox.Checked)
                                await DownloadCompanyQuotes(token);
                        }
                    }
                }
            }
            catch (MySqlException exception)
            {
                Log("MySQL Exception:");
                Log(exception);
            }
            catch (OperationCanceledException)
            {
                Log("Operation Canceled");
            }
            catch (Exception exception)
            {
                Log("Exception:");
                Log(exception);
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
                _connection = null;
                _cancelSource = null;
            }
            EnableStart();
        }

        /// <summary>
        /// Handles the clicking on the Stop button. Cancels the ongoing download of the financial data.
        /// </summary>
        private void stopButton_Click(object sender, EventArgs e)
        {
            if (_cancelSource != null)
            {
                _cancelSource.Cancel();
            }
        }

        private MySqlConnection _connection;
        private CancellationTokenSource _cancelSource;
        private readonly Random _random = new Random();
        private SectorData[] _sectorData;
        private string[] _topCompanyIDs;
        private SortedSet<string> _frozenCompanyProfileIDs;
        private SortedSet<string> _frozenCompanyStatisticsIDs;
        private SortedSet<string> _frozenCompanyQuotesIDs;

        private const int ShortDelay = 100;
        private const int LongDelay = 250;

        /// <summary>
        /// List of all <a href="http://biz.yahoo.com/p/">Yahoo! Finance sectors</a>.
        /// </summary>
        private static readonly Sector[] Sectors = (Sector[]) Enum.GetValues(typeof (Sector));

        /// <summary>
        /// List of all <a href="http://biz.yahoo.com/p/sum_conameu.html">Yahoo! Finance industries</a>.
        /// </summary>
        private static readonly Industry[] Industries = (Industry[]) Enum.GetValues(typeof (Industry));

        /// <summary>
        /// Used globalization culture.
        /// </summary>
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// List of market quote columns.
        /// </summary>
        private static readonly Tuple<string, string, MarketQuoteProperty>[] MarketQuotesColumns =
        {
            new Tuple<string, string, MarketQuoteProperty>("one_day_price_change_percent",
                "One Day Price Change Percent", MarketQuoteProperty.OneDayPriceChangePercent),
            new Tuple<string, string, MarketQuoteProperty>("market_capitalization_in_million",
                "Market Capitalization In Million", MarketQuoteProperty.MarketCapitalizationInMillion),
            new Tuple<string, string, MarketQuoteProperty>("price_earnings_ratio",
                "Price Earnings Ratio", MarketQuoteProperty.PriceEarningsRatio),
            new Tuple<string, string, MarketQuoteProperty>("return_on_equity_percent",
                "Return On Equity Percent", MarketQuoteProperty.ReturnOnEquityPercent),
            new Tuple<string, string, MarketQuoteProperty>("dividend_yield_percent",
                "Dividend Yield Percent", MarketQuoteProperty.DividendYieldPercent),
            new Tuple<string, string, MarketQuoteProperty>("long_term_dept_to_equity",
                "Long Term Dept To Equity", MarketQuoteProperty.LongTermDeptToEquity),
            new Tuple<string, string, MarketQuoteProperty>("price_to_book_value",
                "Price To Book Value", MarketQuoteProperty.PriceToBookValue),
            new Tuple<string, string, MarketQuoteProperty>("net_profit_margin_percent",
                "Net Profit Margin Percent", MarketQuoteProperty.NetProfitMarginPercent),
            new Tuple<string, string, MarketQuoteProperty>("price_to_free_cash_flow",
                "Price To Free Cash Flow", MarketQuoteProperty.PriceToFreeCashFlow)
        };

        /// <summary>
        /// List of market quote column definitions.
        /// </summary>
        private static readonly string MarketQuotesColumnDefinitions = string.Join(", ",
            from column in MarketQuotesColumns
            select string.Format(Culture,
                "`{0}` decimal(20,5) DEFAULT NULL COMMENT '{1}'", column.Item1, column.Item2));

        /// <summary>
        /// Returns a uniform random number from the interval [min, max).
        /// </summary>
        /// <param name="min">Lower bound (inclusive).</param>
        /// <param name="max">Upper bound (exclusive).</param>
        /// <returns>Integer from the interval [min, max).</returns>
        private int UniformRandomDelay(int min, int max)
        {
            return min + (int) ((max - min)*_random.NextDouble());
        }

        /// <summary>
        /// Downloads all sectors and industries.
        /// Creates and fills the following tables: [yahoo_]sector and [yahoo_]industry.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the sectors and industries.</returns>
        private async Task DownloadSectorsAndIndustries(CancellationToken token)
        {
            Log("Downloading Sectors and Industries");
            var response = await Task.Run(() =>
            {
                var md = new MarketDownload();
                return md.DownloadAllSectors();
            }, token);
            _sectorData = response.Result.Items;
            Log(string.Format(Culture,
                "Downloaded {0} sectors:", _sectorData.Length));
            Log(string.Join(Environment.NewLine,
                from sector in _sectorData
                select string.Format(Culture,
                    " * {0} ({1} industries): {2}", sector.Name, sector.Industries.Count,
                    string.Join(", ", from industry in sector.Industries select industry.Name))));
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}sector` (
                  `id` int(10) unsigned NOT NULL COMMENT 'Sector Id',
                  `name` varchar(50) NOT NULL COMMENT 'Sector Name',
                  PRIMARY KEY USING BTREE (`id`),
                  UNIQUE KEY USING BTREE (`name`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='List of Sectors'",
                tablePrefixTextBox.Text), token);
            await ExecuteMultiInsert(string.Format(Culture,
                "REPLACE INTO `{0}sector` (`id`, `name`) VALUES",
                tablePrefixTextBox.Text),
                from sector in _sectorData
                select string.Format(Culture, "(\"{0}\", \"{1}\")",
                    (int) sector.ID, sector.Name), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}industry` (
                  `id` int(10) unsigned NOT NULL COMMENT 'Industry Id',
                  `sector_id` int(10) unsigned NOT NULL COMMENT 'Sector Id',
                  `name` varchar(50) NOT NULL COMMENT 'Industry Name',
                  PRIMARY KEY USING BTREE (`id`),
                  UNIQUE KEY USING BTREE (`name`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='List of Industries'",
                tablePrefixTextBox.Text), token);
            await ExecuteMultiInsert(string.Format(Culture,
                "REPLACE INTO `{0}industry` (`id`, `sector_id`, `name`) VALUES",
                tablePrefixTextBox.Text),
                from sector in _sectorData
                from industry in sector.Industries
                select string.Format(Culture, "(\"{0}\", \"{1}\", \"{2}\")",
                    (int) industry.ID, (int) sector.ID, industry.Name), token);
            Log("Sectors and Industries downloaded successfully!");
        }

        /// <summary>
        /// Downloads all companies.
        /// Creates and fills the following table: [yahoo_]company.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the companies.</returns>
        private async Task DownloadCompanyIDs(CancellationToken token)
        {
            Log("Downloading Company IDs");
            progressBar.Value = 0;
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company` (
                  `id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `name` varchar(100) NOT NULL COMMENT 'Company Name',
                  `sector_id` int(10) unsigned NOT NULL COMMENT 'Sector Id',
                  `industry_id` int(10) unsigned NOT NULL COMMENT 'Industry Id',
                  PRIMARY KEY USING BTREE (`id`),
                  UNIQUE KEY USING BTREE (`name`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='List of Companies'",
                tablePrefixTextBox.Text), token);
            var md = new MarketDownload();
            for (var sectorIndex = 0; sectorIndex < _sectorData.Length; ++sectorIndex)
            {
                var sector = _sectorData[sectorIndex];
                var industries = await Task.Run(() => md.DownloadIndustries(sector.Industries).Result.Items, token);
                await ExecuteMultiInsert(string.Format(Culture,
                    "REPLACE INTO `{0}company` (`id`, `name`, `sector_id`, `industry_id`) VALUES",
                    tablePrefixTextBox.Text),
                    from industry in industries
                    from company in industry.Companies
                    select string.Format(Culture, "(\"{0}\", \"{1}\", \"{2}\", \"{3}\")",
                        company.ID, company.Name, (int) sector.ID, (int) industry.ID), token);
                progressBar.Value = 100*(sectorIndex + 1)/_sectorData.Length;
                await Task.Delay(UniformRandomDelay(LongDelay, 2*LongDelay), token);
            }
            Log("Company IDs downloaded successfully!");
        }

        /// <summary>
        /// Downloads all sector market quotes.
        /// Creates and fills the following table: [yahoo_]sector_market_quotes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the sector market quotes.</returns>
        private async Task DownloadSectorMarketQuotes(CancellationToken token)
        {
            Log("Downloading Sector Market Quotes");
            var mqd = new MarketQuotesDownload();
            var sectors = await Task.Run(() => mqd.DownloadAllSectorQuotes().Result.Items, token);
            await UploadMarketQuotes(string.Format(Culture,
                "{0}sector_market_quotes", tablePrefixTextBox.Text), "sector_name", sectors, token);
            Log("Sector Market Quotes downloaded successfully!");
        }

        /// <summary>
        /// Downloads all industry market quotes.
        /// Creates and fills the following table: [yahoo_]industry_market_quotes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the industry market quotes.</returns>
        private async Task DownloadIndustryMarketQuotes(CancellationToken token)
        {
            Log("Downloading Industry Market Quotes");
            progressBar.Value = 0;
            var mqd = new MarketQuotesDownload();
            for (var sectorIndex = 0; sectorIndex < Sectors.Length; ++sectorIndex)
            {
                var sector = Sectors[sectorIndex];
                var industries = await Task.Run(() => mqd.DownloadIndustryQuotes(sector).Result.Items, token);
                await UploadMarketQuotes(string.Format(Culture,
                    "{0}industry_market_quotes", tablePrefixTextBox.Text), "industry_name", industries, token);
                progressBar.Value = 100*(sectorIndex + 1)/Sectors.Length;
                await Task.Delay(UniformRandomDelay(LongDelay, 2*LongDelay), token);
            }
            Log("Industry Market Quotes downloaded successfully!");
        }

        /// <summary>
        /// Downloads all company market quotes.
        /// Creates and fills the following table: [yahoo_]company_market_quotes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the company market quotes.</returns>
        private async Task DownloadCompanyMarketQuotes(CancellationToken token)
        {
            Log("Downloading Company Market Quotes");
            progressBar.Value = 0;
            var mqd = new MarketQuotesDownload();
            for (var industryIndex = 0; industryIndex < Industries.Length; ++industryIndex)
            {
                var industry = Industries[industryIndex];
                var companies = await Task.Run(() => mqd.DownloadCompanyQuotes(industry).Result.Items.Skip(2), token);
                await UploadMarketQuotes(string.Format(Culture,
                    "{0}company_market_quotes", tablePrefixTextBox.Text), "company_name", companies, token);
                progressBar.Value = 100*(industryIndex + 1)/Industries.Length;
                await Task.Delay(UniformRandomDelay(LongDelay, 2*LongDelay), token);
            }
            Log("Company Market Quotes downloaded successfully!");
        }

        /// <summary>
        /// Uploads the downloaded market quotes to a specified MySQL table.
        /// </summary>
        /// <param name="tableName">MySQL table to be updated.</param>
        /// <param name="columnName">Primary column name.</param>
        /// <param name="marketQuotes">Market quotes to be uploaded.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for uploading the market quotes to a specified MySQL table.</returns>
        private async Task UploadMarketQuotes(string tableName, string columnName,
            IEnumerable<MarketQuotesData> marketQuotes, CancellationToken token)
        {
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}` (
                  `{1}` varchar(50) NOT NULL, {2},
                  PRIMARY KEY USING BTREE (`{1}`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8",
                tableName, columnName, MarketQuotesColumnDefinitions), token);
            await ExecuteMultiInsert(string.Format(Culture,
                "REPLACE INTO `{0}` (`{1}`, {2}) VALUES", tableName, columnName,
                string.Join(", ",
                    from column in MarketQuotesColumns
                    select string.Format(Culture, "`{0}`", column.Item1))),
                from quotes in marketQuotes
                select string.Format(Culture, "(\"{0}\", {1})",
                    quotes.Name, string.Join(", ",
                        from column in MarketQuotesColumns
                        select string.Format(Culture,
                            "{0}", ToDb((double) quotes[column.Item3])))),
                token);
        }

        /// <summary>
        /// Returns the array of company IDs selected from the specified table.
        /// </summary>
        /// <param name="table">Table to be queried.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for returning the array of selected company IDs.</returns>
        private async Task<string[]> GetCompanyIDs(string table, CancellationToken token)
        {
            try
            {
                return await ExecuteColumnQuery(string.Format(Culture,
                    "SELECT DISTINCT company_id AS id FROM {0}{1} ORDER BY company_id",
                    tablePrefixTextBox.Text, table), token);
            }
            catch (MySqlException exception)
            {
                Log(string.Format(Culture,
                    "MySQL Exception when reading from table: {0}{1}",
                    tablePrefixTextBox.Text, table));
                Log(exception);
                // Return an empty array.
                return new string[] {};
            }
        }

        /// <summary>
        /// Returns the array of the top company IDs (based on the market capitalization).
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for returning the array of the top company IDs.</returns>
        private async Task<string[]> GetTopCompanyIDs(CancellationToken token)
        {
            try
            {
                return await ExecuteColumnQuery(string.Format(Culture, @"
                    SELECT c.id AS id
                    FROM {0}company c JOIN 
                         {0}company_market_quotes cq ON 
                            (c.name = cq.company_name)
                    WHERE c.id NOT LIKE ""%.%""
                    ORDER BY market_capitalization_in_million DESC
                    LIMIT {1}", tablePrefixTextBox.Text, topStocksNumericUpDown.Value), token);
            }
            catch (MySqlException exception)
            {
                Log("MySQL Exception when reading the top company IDs");
                Log(exception);
                // Return an empty array.
                return new string[] {};
            }
        }

        /// <summary>
        /// Splits the given enumerable source to the enumeration of smaller chunks.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source enumeration.</typeparam>
        /// <param name="source">Source enumeration.</param>
        /// <param name="chunkSize">Size of the resulting chunks.</param>
        /// <returns>Enumeration of the chunks containing the original elements.</returns>
        private static IEnumerable<T[]> Chunk<T>(IEnumerable<T> source, int chunkSize)
        {
            var buffer = new List<T>(chunkSize);
            foreach (var element in source)
            {
                buffer.Add(element);
                if (buffer.Count != chunkSize) continue;
                yield return buffer.ToArray();
                buffer.Clear();
            }
            if (buffer.Count > 0)
                yield return buffer.ToArray();
        }

        /// <summary>
        /// Downloads the company profiles for the top company IDs (based on the market capitalization).
        /// Creates and fills the following tables:
        ///     [yahoo_]company_profile, [yahoo_]company_websites,
        ///     [yahoo_]company_executives, [yahoo_]company_valuation_measures,
        ///     [yahoo_]company_financial_highlights, [yahoo_]company_trading_info. 
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the company profiles.</returns>
        private async Task DownloadCompanyProfile(CancellationToken token)
        {
            Log("Downloading Company Profiles");
            progressBar.Value = 0;
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_profile` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `company_name` varchar(100) NOT NULL COMMENT 'Company Name',
                  `sector_id` int(10) unsigned DEFAULT NULL COMMENT 'Sector Id',
                  `industry_id` int(10) unsigned DEFAULT NULL COMMENT 'Industry Id',
                  `address` varchar(5000) NOT NULL COMMENT 'Company Address',
                  `business_summary` varchar(5000) NOT NULL COMMENT 'Business Summary',
                  `corporate_governance` varchar(5000) NOT NULL COMMENT 'Corporate Governance',
                  `full_time_employees` int(10) unsigned DEFAULT NULL COMMENT 'Full-Time Employees',
                  PRIMARY KEY USING BTREE (`company_id`),
                  UNIQUE KEY USING BTREE (`company_name`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Profile'",
                tablePrefixTextBox.Text), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_websites` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `absolute_uri` varchar(1000) NOT NULL COMMENT 'Absolute Uri',
                  KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Websites'",
                tablePrefixTextBox.Text), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_executives` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `name` varchar(5000) NOT NULL COMMENT 'Name',
                  `position` varchar(5000) NOT NULL COMMENT 'Position',
                  `pay` decimal(20,5) DEFAULT NULL COMMENT 'Pay',
                  KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Executives'",
                tablePrefixTextBox.Text), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_valuation_measures` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `enterprise_value_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Enterprise Value In Million',
                  `enterprise_value_to_ebitda` decimal(20,5) DEFAULT NULL
                    COMMENT 'Enterprise Value To EBITDA',
                  `enterprise_value_to_revenue` decimal(20,5) DEFAULT NULL
                    COMMENT 'Enterprise Value To Revenue',
                  `forward_pe` decimal(20,5) DEFAULT NULL
                    COMMENT 'Forward PE',
                  `market_capitalisation_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Market Capitalisation In Million',
                  `peg_ratio` decimal(20,5) DEFAULT NULL
                    COMMENT 'PEG Ratio',
                  `price_to_book` decimal(20,5) DEFAULT NULL
                    COMMENT 'Price To Book',
                  `price_to_sales` decimal(20,5) DEFAULT NULL
                    COMMENT 'Price To Sales',
                  `trailing_pe` decimal(20,5) DEFAULT NULL
                    COMMENT 'Trailing PE',
                  PRIMARY KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Valuation Measures'",
                tablePrefixTextBox.Text), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_financial_highlights` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `book_value_per_share` decimal(20,5) DEFAULT NULL
                    COMMENT 'Book Value Per Share',
                  `current_ratio` decimal(20,5) DEFAULT NULL
                    COMMENT 'Current Ratio',
                  `diluted_eps` decimal(20,5) DEFAULT NULL
                    COMMENT 'Diluted EPS',
                  `ebitda_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'EBITDA In Million',
                  `fiscal_year_ends` date DEFAULT NULL
                    COMMENT 'Fiscal Year Ends',
                  `gross_profit_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Gross Profit In Million',
                  `levered_free_cash_flow_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Levered Free Cash Flow In Million',
                  `most_recent_quarter` date DEFAULT NULL
                    COMMENT 'Most Recent Quarter',
                  `net_income_avl_to_common_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Net Income Avl To Common In Million',
                  `operating_cash_flow_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Operating Cash Flow In Million',
                  `operating_margin_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Operating Margin Percent',
                  `profit_margin_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Profit Margin Percent',
                  `quarterly_revenue_growth_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Quarterly Revenue Growth Percent',
                  `quaterly_earnings_growth_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Quaterly Earnings Growth Percent',
                  `return_on_assets_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Return On Assets Percent',
                  `return_on_equity_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Return On Equity Percent',
                  `revenue_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Revenue In Million',
                  `revenue_per_share` decimal(20,5) DEFAULT NULL
                    COMMENT 'Revenue Per Share',
                  `total_cash_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Total Cash In Million',
                  `total_cash_per_share` decimal(20,5) DEFAULT NULL
                    COMMENT 'Total Cash Per Share',
                  `total_debt_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Total Debt In Million',
                  `total_debt_per_equity` decimal(20,5) DEFAULT NULL
                    COMMENT 'Total Debt Per Equity',
                  PRIMARY KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Financial Highlights'",
                tablePrefixTextBox.Text), token);
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_trading_info` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `average_volume_ten_days_in_thousand` decimal(20,5) DEFAULT NULL
                    COMMENT 'Average Volume Ten Days In Thousand',
                  `average_volume_three_month_in_thousand` decimal(20,5) DEFAULT NULL
                    COMMENT 'Average Volume Three Month In Thousand',
                  `beta` decimal(20,5) DEFAULT NULL
                    COMMENT 'Beta',
                  `dividend_date` date DEFAULT NULL
                    COMMENT 'Dividend Date',
                  `ex_dividend_date` date DEFAULT NULL
                    COMMENT 'Ex Dividend Date',
                  `fifty_day_moving_average` decimal(20,5) DEFAULT NULL
                    COMMENT 'Fifty Day Moving Average',
                  `five_year_average_dividend_yield_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Five Year Average Dividend Yield Percent',
                  `float_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Float In Million',
                  `forward_annual_dividend_rate` decimal(20,5) DEFAULT NULL
                    COMMENT 'Forward Annual Dividend Rate',
                  `forward_annual_dividend_yield_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Forward Annual Dividend Yield Percent',
                  `last_split_date` date DEFAULT NULL
                    COMMENT 'Last Split Date',
                  `last_split_factor_new_shares` int(10) unsigned DEFAULT NULL
                    COMMENT 'Last Split Factor - New Shares',
                  `last_split_factor_old_shares` int(10) unsigned DEFAULT NULL
                    COMMENT 'Last Split Factor - Old Shares',
                  `one_year_change_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'One Year Change Percent',
                  `one_year_high` decimal(20,5) DEFAULT NULL
                    COMMENT 'One Year High',
                  `one_year_low` decimal(20,5) DEFAULT NULL
                    COMMENT 'One Year Low',
                  `payout_ratio` decimal(20,5) DEFAULT NULL
                    COMMENT 'Payout Ratio',
                  `percent_held_by_insiders` decimal(20,5) DEFAULT NULL
                    COMMENT 'Percent Held By Insiders',
                  `percent_held_by_institutions` decimal(20,5) DEFAULT NULL
                    COMMENT 'Percent Held By Institutions',
                  `shares_outstanding_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Shares Outstanding In Million',
                  `shares_short_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Shares Short In Million',
                  `shares_short_prior_month_in_million` decimal(20,5) DEFAULT NULL
                    COMMENT 'Shares Short Prior Month In Million',
                  `short_percent_of_float` decimal(20,5) DEFAULT NULL
                    COMMENT 'Short Percent Of Float',
                  `short_ratio` decimal(20,5) DEFAULT NULL
                    COMMENT 'Short Ratio',
                  `sp500_one_year_change_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'SP500 One Year Change Percent',
                  `trailing_annual_dividend_yield` decimal(20,5) DEFAULT NULL
                    COMMENT 'Trailing Annual Dividend Yield',
                  `trailing_annual_dividend_yield_percent` decimal(20,5) DEFAULT NULL
                    COMMENT 'Trailing Annual Dividend Yield Percent',
                  `two_hundred_day_moving_average` decimal(20,5) DEFAULT NULL
                    COMMENT 'Two Hundred Day Moving Average',
                  PRIMARY KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Trading Info'",
                tablePrefixTextBox.Text), token);
            const int chunkSize = 10;
            var chunks = Chunk(_topCompanyIDs, chunkSize).ToArray();
            var cpd = new CompanyProfileDownload();
            var csd = new CompanyStatisticsDownload();
            var companyProfileDataList = new List<CompanyProfileData>(chunkSize);
            var companyStatisticsDataList = new List<CompanyStatisticsData>(chunkSize);
            for (var chunkIndex = 0; chunkIndex < chunks.Length; ++chunkIndex)
            {
                var chunk = chunks[chunkIndex];
                companyStatisticsDataList.Clear();
                companyStatisticsDataList.Clear();
                await Task.Run(() =>
                {
                    foreach (var company in chunk)
                    {
                        token.ThrowIfCancellationRequested();
                        if (!_frozenCompanyProfileIDs.Contains(company))
                        {
                            try
                            {
                                var cpr = cpd.Download(company);
                                if (cpr.Result == null || cpr.Result.Item == null)
                                    Log(string.Format(Culture, "Could not download the profile for: {0}", company));
                                else
                                {
                                    var profile = cpr.Result.Item;
                                    if (!profile.Details.Sector.HasValue || !profile.Details.Industry.HasValue)
                                        Log(string.Format(Culture,
                                            "Company profile for: {0} does not contain the sector id / industry id",
                                            company));
                                    companyProfileDataList.Add(profile);
                                }
                                // It is important to free the used memory. Otherwise, we could get OutOfMemoryException!
                                // ReSharper disable once RedundantAssignment
                                cpr = null;
                            }
                            catch (Exception exception)
                            {
                                Log(string.Format(Culture, "Exception when downloading the profile for: {0}", company));
                                Log(exception);
                            }
                            Thread.Sleep(ShortDelay/2);
                        }
                        token.ThrowIfCancellationRequested();
                        if (!_frozenCompanyStatisticsIDs.Contains(company))
                        {
                            try
                            {
                                var csr = csd.Download(company);
                                if (csr.Result == null || csr.Result.Item == null)
                                    Log(string.Format(Culture, "Could not download the statistics for: {0}", company));
                                else companyStatisticsDataList.Add(csr.Result.Item);
                                // It is important to free the used memory. Otherwise, we could get OutOfMemoryException!
                                // ReSharper disable once RedundantAssignment
                                csr = null;
                            }
                            catch (Exception exception)
                            {
                                Log(string.Format(Culture, "Exception when downloading the statistics for: {0}", company));
                                Log(exception);
                            }
                            Thread.Sleep(ShortDelay/2);
                        }
                    }
                }, token);
                try
                {
                    await ExecuteMultiInsert(string.Format(Culture, @"
                        REPLACE INTO `{0}company_profile` (
                            `company_id`, `company_name`, `sector_id`, `industry_id`,
                            `address`, `business_summary`, `corporate_governance`,
                            `full_time_employees`) VALUES", tablePrefixTextBox.Text),
                        from profile in companyProfileDataList
                        let sector = profile.Details.Sector
                        let industry = profile.Details.Industry
                        select string.Format(Culture, "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                            ToDb(profile.ID), ToDb(profile.CompanyName),
                            sector.HasValue ? ((int) sector.Value).ToString(Culture) : "NULL",
                            industry.HasValue ? ((int) industry.Value).ToString(Culture) : "NULL",
                            ToDb(profile.Address), ToDb(profile.BusinessSummary),
                            ToDb(profile.CorporateGovernance),
                            ToDb(profile.Details.FullTimeEmployees)), token);
                    await ExecuteMultiInsert(string.Format(Culture,
                        "REPLACE INTO `{0}company_websites` (`company_id`, `absolute_uri`) VALUES",
                        tablePrefixTextBox.Text),
                        from profile in companyProfileDataList
                        from website in profile.CompanyWebsites
                        select string.Format(Culture, "({0}, {1})",
                            ToDb(profile.ID), ToDb(website.AbsoluteUri)), token);
                    await ExecuteMultiInsert(string.Format(Culture,
                        "REPLACE INTO `{0}company_executives` (`company_id`, `name`, `position`, `pay`) VALUES",
                        tablePrefixTextBox.Text),
                        from profile in companyProfileDataList
                        from executive in profile.KeyExecutives
                        select string.Format(Culture, "({0}, {1}, {2}, {3})",
                            ToDb(profile.ID), ToDb(executive.Name),
                            ToDb(executive.Position), ToDb(executive.Pay)), token);
                }
                catch (MySqlException exception)
                {
                    Log(string.Format(Culture, "MySQL Exception when storing the profile for [{0}]:",
                        string.Join(", ", chunk)));
                    Log(exception);
                }
                try
                {
                    await ExecuteMultiInsert(string.Format(Culture, @"
                        REPLACE INTO `{0}company_valuation_measures` (`company_id`,
                            `enterprise_value_in_million`, `enterprise_value_to_ebitda`, `enterprise_value_to_revenue`,
                            `forward_pe`, `market_capitalisation_in_million`, `peg_ratio`,
                            `price_to_book`, `price_to_sales`, `trailing_pe`)
                        VALUES", tablePrefixTextBox.Text),
                        from statistics in companyStatisticsDataList
                        let measures = statistics.ValuationMeasures
                        select string.Format(Culture,
                            "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", ToDb(statistics.ID),
                            ToDb(measures.EnterpriseValueInMillion), ToDb(measures.EnterpriseValueToEBITDA),
                            ToDb(measures.EnterpriseValueToRevenue), ToDb(measures.ForwardPE),
                            ToDb(measures.MarketCapitalisationInMillion), ToDb(measures.PEGRatio),
                            ToDb(measures.PriceToBook), ToDb(measures.PriceToSales),
                            ToDb(measures.TrailingPE)), token);
                    await ExecuteMultiInsert(string.Format(Culture, @"
                        REPLACE INTO `{0}company_financial_highlights` (`company_id`,
                            `book_value_per_share`, `current_ratio`, `diluted_eps`, `ebitda_in_million`,
                            `fiscal_year_ends`, `gross_profit_in_million`, `levered_free_cash_flow_in_million`,
                            `most_recent_quarter`, `net_income_avl_to_common_in_million`, `operating_cash_flow_in_million`,
                            `operating_margin_percent`, `profit_margin_percent`, `quarterly_revenue_growth_percent`,
                            `quaterly_earnings_growth_percent`, `return_on_assets_percent`, `return_on_equity_percent`,
                            `revenue_in_million`, `revenue_per_share`, `total_cash_in_million`, `total_cash_per_share`,
                            `total_debt_in_million`, `total_debt_per_equity`)
                        VALUES", tablePrefixTextBox.Text),
                        from statistics in companyStatisticsDataList
                        let highlights = statistics.FinancialHighlights
                        select string.Format(Culture,
                            "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, " +
                            "{10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, " +
                            "{20}, {21}, {22})", ToDb(statistics.ID),
                            ToDb(highlights.BookValuePerShare), ToDb(highlights.CurrentRatio),
                            ToDb(highlights.DilutedEPS), ToDb(highlights.EBITDAInMillion),
                            ToDb(highlights.FiscalYearEnds), ToDb(highlights.GrossProfitInMillion),
                            ToDb(highlights.LeveredFreeCashFlowInMillion), ToDb(highlights.MostRecentQuarter),
                            ToDb(highlights.NetIncomeAvlToCommonInMillion), ToDb(highlights.OperatingCashFlowInMillion),
                            ToDb(highlights.OperatingMarginPercent), ToDb(highlights.ProfitMarginPercent),
                            ToDb(highlights.QuarterlyRevenueGrowthPercent),
                            ToDb(highlights.QuaterlyEarningsGrowthPercent),
                            ToDb(highlights.ReturnOnAssetsPercent), ToDb(highlights.ReturnOnEquityPercent),
                            ToDb(highlights.RevenueInMillion), ToDb(highlights.RevenuePerShare),
                            ToDb(highlights.TotalCashInMillion), ToDb(highlights.TotalCashPerShare),
                            ToDb(highlights.TotalDeptInMillion), ToDb(highlights.TotalDeptPerEquity)), token);
                    await ExecuteMultiInsert(string.Format(Culture, @"
                        REPLACE INTO `{0}company_trading_info` ( `company_id`,
                            `average_volume_ten_days_in_thousand`, `average_volume_three_month_in_thousand`,
                            `beta`, `dividend_date`, `ex_dividend_date`, `fifty_day_moving_average`,
                            `five_year_average_dividend_yield_percent`, `float_in_million`,
                            `forward_annual_dividend_rate`, `forward_annual_dividend_yield_percent`,
                            `last_split_date`, `last_split_factor_new_shares`, `last_split_factor_old_shares`,
                            `one_year_change_percent`, `one_year_high`, `one_year_low`, `payout_ratio`,
                            `percent_held_by_insiders`, `percent_held_by_institutions`,
                            `shares_outstanding_in_million`, `shares_short_in_million`,
                            `shares_short_prior_month_in_million`, `short_percent_of_float`, `short_ratio`,
                            `sp500_one_year_change_percent`, `trailing_annual_dividend_yield`,
                            `trailing_annual_dividend_yield_percent`, `two_hundred_day_moving_average`)
                        VALUES", tablePrefixTextBox.Text),
                        from statistics in companyStatisticsDataList
                        let trading = statistics.TradingInfo
                        let lastSplitFactorNewShares =
                            trading.LastSplitFactor != null ? trading.LastSplitFactor.NewShares : 1
                        let lastSplitFactorOldShares =
                            trading.LastSplitFactor != null ? trading.LastSplitFactor.OldShares : 1
                        select string.Format(Culture,
                            "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, " +
                            "{10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, " +
                            "{20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28})", ToDb(statistics.ID),
                            ToDb(trading.AverageVolumeTenDaysInThousand),
                            ToDb(trading.AverageVolumeThreeMonthInThousand),
                            ToDb(trading.Beta), ToDb(trading.DividendDate), ToDb(trading.ExDividendDate),
                            ToDb(trading.FiftyDayMovingAverage), ToDb(trading.FiveYearAverageDividendYieldPercent),
                            ToDb(trading.FloatInMillion), ToDb(trading.ForwardAnnualDividendRate),
                            ToDb(trading.ForwardAnnualDividendYieldPercent), ToDb(trading.LastSplitDate),
                            lastSplitFactorNewShares, lastSplitFactorOldShares, ToDb(trading.OneYearChangePercent),
                            ToDb(trading.OneYearHigh), ToDb(trading.OneYearLow), ToDb(trading.PayoutRatio),
                            ToDb(trading.PercentHeldByInsiders), ToDb(trading.PercentHeldByInstitutions),
                            ToDb(trading.SharesOutstandingInMillion), ToDb(trading.SharesShortInMillion),
                            ToDb(trading.SharesShortPriorMonthInMillion), ToDb(trading.ShortPercentOfFloat),
                            ToDb(trading.ShortRatio), ToDb(trading.SP500OneYearChangePercent),
                            ToDb(trading.TrailingAnnualDividendYield), ToDb(trading.TrailingAnnualDividendYieldPercent),
                            ToDb(trading.TwoHundredDayMovingAverage)), token);
                }
                catch (MySqlException exception)
                {
                    Log(string.Format(Culture, "MySQL Exception when storing the statistics for [{0}]:",
                        string.Join(", ", chunk)));
                    Log(exception);
                }
                progressBar.Value = 100*(chunkIndex + 1)/chunks.Length;
                await Task.Delay(UniformRandomDelay(LongDelay, 2*LongDelay), token);
            }
            // Fix the missing sector ids / industry ids.
            await ExecuteNonQuery(string.Format(Culture, @"
                UPDATE {0}company c JOIN {0}company_profile cp ON (c.id = cp.company_id)
                SET cp.sector_id = c.sector_id, cp.industry_id = c.industry_id
                WHERE cp.sector_id IS NULL OR cp.industry_id IS NULL",
                tablePrefixTextBox.Text), token);
            Log("Company Profiles downloaded successfully!");
        }

        /// <summary>
        /// Downloads the daily historical stock data for the top company IDs (based on the market capitalization).
        /// Creates and fills the table: [yahoo_]company_quotes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for downloading the daily historical stock data.</returns>
        private async Task DownloadCompanyQuotes(CancellationToken token)
        {
            Log("Downloading Company Quotes");
            await ExecuteNonQuery(string.Format(Culture, @"
                CREATE TABLE IF NOT EXISTS `{0}company_quotes` (
                  `company_id` varchar(20) NOT NULL COMMENT 'Company Id',
                  `date` date NOT NULL COMMENT 'Trading Date',
                  `open` decimal(20,5) NOT NULL COMMENT 'Open',
                  `high` decimal(20,5) NOT NULL COMMENT 'High',
                  `low` decimal(20,5) NOT NULL COMMENT 'Low',
                  `close` decimal(20,5) NOT NULL COMMENT 'Close',
                  `close_adjusted` decimal(20,5) NOT NULL COMMENT 'Close Adjusted',
                  `volume` decimal(20,5) NOT NULL COMMENT 'Volume',
                  PRIMARY KEY USING BTREE (`company_id`, `date`),
                  KEY USING BTREE (`company_id`)
                ) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Company Quotes'",
                tablePrefixTextBox.Text), token);
            var quoteMultiInsertPrefix = string.Format(Culture, @"
                REPLACE INTO `{0}company_quotes` (
                    `company_id`, `date`, `open`, `high`, `low`, `close`, `close_adjusted`, `volume`)
                VALUES", tablePrefixTextBox.Text);
            const int chunkSize = 10;
            var chunks = Chunk(
                from company in _topCompanyIDs
                where !_frozenCompanyQuotesIDs.Contains(company)
                select company, chunkSize).ToArray();
            var hqd = new HistQuotesDownload();
            for (var chunkIndex = 0; chunkIndex < chunks.Length; ++chunkIndex)
            {
                var chunk = chunks[chunkIndex];
                await Task.Run(async () =>
                {
                    foreach (var company in chunk)
                    {
                        token.ThrowIfCancellationRequested();
                        try
                        {
                            var hqr = hqd.Download(company, new DateTime(1900, 1, 1), DateTime.Today,
                                HistQuotesInterval.Daily);
                            if (hqr.Result == null || hqr.Result.Items == null || hqr.Result.Items.Count == 0)
                                Log(string.Format(Culture, "Could not download the quotes for: {0}", company));
                            else
                            {
                                // ReSharper disable once AccessToForEachVariableInClosure
                                await ExecuteMultiInsert(quoteMultiInsertPrefix,
                                    from quote in hqr.Result.Items
                                    select string.Format(Culture, "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                                        ToDb(company), ToDb(quote.TradingDate),
                                        ToDb(quote.Open), ToDb(quote.High), ToDb(quote.Low), ToDb(quote.Close),
                                        ToDb(quote.CloseAdjusted), quote.Volume), token);
                            }
                            // It is important to free the used memory. Otherwise, we could get OutOfMemoryException!
                            // ReSharper disable once RedundantAssignment
                            hqr = null;
                        }
                        catch (MySqlException exception)
                        {
                            Log(string.Format(Culture, "MySQL Exception when storing the quotes for {0}:", company));
                            Log(exception);
                        }
                        catch (Exception exception)
                        {
                            Log(string.Format(Culture, "Exception when downloading the quotes for: {0}", company));
                            Log(exception);
                        }
                        Thread.Sleep(ShortDelay);
                    }
                }, token);
                progressBar.Value = 100*(chunkIndex + 1)/chunks.Length;
                await Task.Delay(UniformRandomDelay(LongDelay, 2*LongDelay), token);
            }
            Log("Company Quotes downloaded successfully!");
        }

        /// <summary>
        /// Converts the given floating-point number to a MySQL friendly string.
        /// </summary>
        /// <param name="number">Floating-point number to be converted.</param>
        /// <returns>MySQL friendly string.</returns>
        private static string ToDb(double number)
        {
            return double.IsNaN(number) ? "NULL" : "\"" + number.ToString("G", Culture) + "\"";
        }

        /// <summary>
        /// Converts the given string to a MySQL friendly string.
        /// </summary>
        /// <param name="text">String to be converted.</param>
        /// <returns>MySQL friendly string.</returns>
        private static string ToDb(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "\"\"" : "\"" + text.Replace("\"", "\\\"") + "\"";
        }

        /// <summary>
        /// Converts the given date to a MySQL friendly string.
        /// </summary>
        /// <param name="date">Date to be converted.</param>
        /// <returns>MySQL friendly string.</returns>
        private static string ToDb(DateTime date)
        {
            return date > new DateTime(1900, 1, 1) ? "\"" + date.ToString("yyyy-MM-dd") + "\"" : "NULL";
        }

        /// <summary>
        /// Asynchronously executes the given MySQL query that selects strings from one column.
        /// </summary>
        /// <param name="query">MySQL query that selects strings from one column.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task returning the array of selected strings.</returns>
        private async Task<string[]> ExecuteColumnQuery(string query, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return await Task.Run(async () =>
            {
                using (var command = new MySqlCommand {Connection = _connection, CommandText = query})
                {
                    command.Prepare();
                    // Unfortunatelly, CancellationToken does not work well with the current version of MySql.Data!
                    // ReSharper disable MethodSupportsCancellation
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var result = new List<string>();
                        while (await reader.ReadAsync())
                        {
                            token.ThrowIfCancellationRequested();
                            result.Add(reader.GetString(0));
                        }
                        return result.ToArray();
                    }
                    // ReSharper restore MethodSupportsCancellation
                }
            }, token);
        }

        /// <summary>
        /// Asynchronously executes the given MySQL non-query.
        /// </summary>
        /// <param name="query">MySQL query.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for executing the given MySQL query.</returns>
        private async Task<int> ExecuteNonQuery(string query, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            using (var command = new MySqlCommand {Connection = _connection, CommandText = query})
            {
                command.Prepare();
                // Unfortunatelly, CancellationToken does not work well with the current version of MySql.Data!
                // ReSharper disable once MethodSupportsCancellation
                return await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Asynchronously executes the given multi-insert MySQL query.
        /// </summary>
        /// <param name="query">MySQL (insert / replace) query prefix.</param>
        /// <param name="values">Enumeration of the rows to be inserted.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Task for executing the given multi-insert MySQL query.</returns>
        private async Task<int> ExecuteMultiInsert(string query, IEnumerable<string> values, CancellationToken token)
        {
            // We run the multi-insert in a separate thread, as it is mostly CPU-bound!
            token.ThrowIfCancellationRequested();
            return await Task.Run(async () =>
            {
                const int capacity = 100;
                var buffer = new List<string>(capacity);
                Func<int, Task<int>> flush = async minCapacity =>
                {
                    if (buffer.Count < minCapacity) return 0;
                    var numberOfRows = await ExecuteNonQuery(
                        query + Environment.NewLine + string.Join(", ", buffer), token);
                    buffer.Clear();
                    return numberOfRows;
                };
                var totalNumberOfRows = 0;
                foreach (var value in values)
                {
                    token.ThrowIfCancellationRequested();
                    buffer.Add(value);
                    totalNumberOfRows += await flush(capacity);
                }
                totalNumberOfRows += await flush(1);
                return totalNumberOfRows;
            }, token);
        }

        /// <summary>
        /// Sets the form to the initial state. Enables the Start button.
        /// </summary>
        private void EnableStart()
        {
            startButton.Enabled = true;
            stopButton.Enabled = false;
            mysqlGroupBox.Enabled = true;
        }

        /// <summary>
        /// Prepares the form for the asynchronous download. Enables the Stop button.
        /// </summary>
        private void EnableStop()
        {
            startButton.Enabled = false;
            stopButton.Enabled = true;
            mysqlGroupBox.Enabled = false;
        }

        /// <summary>
        /// Thread-safe method for logging the exception.
        /// </summary>
        /// <param name="exception">Exception to be logged.</param>
        private void Log(Exception exception)
        {
            var message = exception.Message;
            if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                message += Environment.NewLine + exception.InnerException.Message;
            if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                message += Environment.NewLine + exception.StackTrace;
            Log(message);
        }

        /// <summary>
        /// Thread-safe method for logging the message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        private void Log(string message)
        {
            Invoke(new Action(() => logTextBox.AppendText(message + Environment.NewLine)));
        }
    }
}
