
YahooDownloader
===============

YahooDownloader is a simple .NET WinForms application for downloading financial data from [finance.yahoo.com](http://finance.yahoo.com/) by using [Yahoo! Managed](https://code.google.com/p/yahoo-finance-managed/) and saving them into a MySQL database by using [MySql.Data](https://www.nuget.org/packages/MySql.Data/) connector.

Bitcoin Donation Address: `1Poh8wnPWWadC5i6LNGESK2Q1Z6351HkRd`

**Prerequisites:**

- [VisualStudio](http://www.visualstudio.com/)
- [.NET 4.5](http://www.microsoft.com/en-us/download/details.aspx?id=30653)
- [Yahoo! Managed](https://code.google.com/p/yahoo-finance-managed/) - included.
- [MySql.Data](https://www.nuget.org/packages/MySql.Data/) - included.

Downloaded Tables
-----------------

**Basic tables**:
- `yahoo_sector` - List of [sectors](http://biz.yahoo.com/p/).
- `yahoo_industry` - List of [industries](http://biz.yahoo.com/p/sum_conameu.html) including their sector id.
- `yahoo_company` - List of companies including their sector id and industry ids.

**Market quotes**:
- `yahoo_sector_market_quotes` - Sector market quotes.
- `yahoo_industry_market_quotes` - Industry market quotes.
- `yahoo_company_market_quotes` - Company market quotes.

**Fundamental and trading info**:
- `yahoo_company_profile` - Company profiles (address, business summary, etc.).
- `yahoo_company_executives` - Company executives.
- `yahoo_company_websites` - Company websites.
- `yahoo_company_financial_highlights` - Company financial highlights.
- `yahoo_company_valuation_measures` - Company valuation measures.
- `yahoo_company_trading_info` - Company trading info.

**Historical stock data**:
- `yahoo_company_quotes` - Daily historical stock data.

*NOTE*: The prefix (in this case `yahoo_`) can be specified.

LICENSE
=======

YahooDownloader is licensed to you under MIT.X11:

Copyright (c) 2015 Peter Cerno

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.