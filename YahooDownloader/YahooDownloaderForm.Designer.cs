namespace YahooDownloader
{
    partial class YahooDownloaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mysqlGroupBox = new System.Windows.Forms.GroupBox();
            this.tablePrefixTextBox = new System.Windows.Forms.TextBox();
            this.tablePrefixLabel = new System.Windows.Forms.Label();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.hostLabel = new System.Windows.Forms.Label();
            this.downloadGroupBox = new System.Windows.Forms.GroupBox();
            this.downloadMissingTopStocksCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadCompanyQuotesCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadCompanyProfileCheckBox = new System.Windows.Forms.CheckBox();
            this.topStocksNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.downloadCompanyMarketQuotesCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadIndustryMarketQuotesCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadSectorMarketQuotesCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadCompanyIDsCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadSectorsAndIndustriesCheckBox = new System.Windows.Forms.CheckBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.logGroupBox = new System.Windows.Forms.GroupBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.downloadTopStocksRadioButton = new System.Windows.Forms.RadioButton();
            this.downloadStocksFromTableRadioButton = new System.Windows.Forms.RadioButton();
            this.stocksTableNameTextBox = new System.Windows.Forms.TextBox();
            this.downloadSelectedStocksRadioButton = new System.Windows.Forms.RadioButton();
            this.selectedStocksTextBox = new System.Windows.Forms.TextBox();
            this.mysqlGroupBox.SuspendLayout();
            this.downloadGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topStocksNumericUpDown)).BeginInit();
            this.logGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mysqlGroupBox
            // 
            this.mysqlGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mysqlGroupBox.Controls.Add(this.tablePrefixTextBox);
            this.mysqlGroupBox.Controls.Add(this.tablePrefixLabel);
            this.mysqlGroupBox.Controls.Add(this.databaseTextBox);
            this.mysqlGroupBox.Controls.Add(this.databaseLabel);
            this.mysqlGroupBox.Controls.Add(this.passwordTextBox);
            this.mysqlGroupBox.Controls.Add(this.passwordLabel);
            this.mysqlGroupBox.Controls.Add(this.userTextBox);
            this.mysqlGroupBox.Controls.Add(this.userLabel);
            this.mysqlGroupBox.Controls.Add(this.hostTextBox);
            this.mysqlGroupBox.Controls.Add(this.hostLabel);
            this.mysqlGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mysqlGroupBox.Name = "mysqlGroupBox";
            this.mysqlGroupBox.Size = new System.Drawing.Size(660, 150);
            this.mysqlGroupBox.TabIndex = 0;
            this.mysqlGroupBox.TabStop = false;
            this.mysqlGroupBox.Text = "MySQL Configuration";
            // 
            // tablePrefixTextBox
            // 
            this.tablePrefixTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablePrefixTextBox.Location = new System.Drawing.Point(78, 123);
            this.tablePrefixTextBox.Name = "tablePrefixTextBox";
            this.tablePrefixTextBox.Size = new System.Drawing.Size(576, 20);
            this.tablePrefixTextBox.TabIndex = 9;
            this.tablePrefixTextBox.Text = "yahoo_";
            // 
            // tablePrefixLabel
            // 
            this.tablePrefixLabel.AutoSize = true;
            this.tablePrefixLabel.Location = new System.Drawing.Point(6, 126);
            this.tablePrefixLabel.Name = "tablePrefixLabel";
            this.tablePrefixLabel.Size = new System.Drawing.Size(66, 13);
            this.tablePrefixLabel.TabIndex = 8;
            this.tablePrefixLabel.Text = "Table Prefix:";
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseTextBox.Location = new System.Drawing.Point(78, 97);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(576, 20);
            this.databaseTextBox.TabIndex = 7;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(6, 100);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(56, 13);
            this.databaseLabel.TabIndex = 6;
            this.databaseLabel.Text = "Database:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(78, 71);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(576, 20);
            this.passwordTextBox.TabIndex = 5;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(6, 74);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password:";
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(78, 45);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(576, 20);
            this.userTextBox.TabIndex = 3;
            this.userTextBox.Text = "root";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(6, 48);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(32, 13);
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "User:";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostTextBox.Location = new System.Drawing.Point(78, 19);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(576, 20);
            this.hostTextBox.TabIndex = 1;
            this.hostTextBox.Text = "localhost";
            // 
            // hostLabel
            // 
            this.hostLabel.AutoSize = true;
            this.hostLabel.Location = new System.Drawing.Point(6, 22);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(32, 13);
            this.hostLabel.TabIndex = 0;
            this.hostLabel.Text = "Host:";
            // 
            // downloadGroupBox
            // 
            this.downloadGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadGroupBox.Controls.Add(this.selectedStocksTextBox);
            this.downloadGroupBox.Controls.Add(this.downloadSelectedStocksRadioButton);
            this.downloadGroupBox.Controls.Add(this.stocksTableNameTextBox);
            this.downloadGroupBox.Controls.Add(this.downloadStocksFromTableRadioButton);
            this.downloadGroupBox.Controls.Add(this.downloadTopStocksRadioButton);
            this.downloadGroupBox.Controls.Add(this.downloadMissingTopStocksCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadCompanyQuotesCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadCompanyProfileCheckBox);
            this.downloadGroupBox.Controls.Add(this.topStocksNumericUpDown);
            this.downloadGroupBox.Controls.Add(this.downloadCompanyMarketQuotesCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadIndustryMarketQuotesCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadSectorMarketQuotesCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadCompanyIDsCheckBox);
            this.downloadGroupBox.Controls.Add(this.downloadSectorsAndIndustriesCheckBox);
            this.downloadGroupBox.Controls.Add(this.stopButton);
            this.downloadGroupBox.Controls.Add(this.startButton);
            this.downloadGroupBox.Location = new System.Drawing.Point(12, 168);
            this.downloadGroupBox.Name = "downloadGroupBox";
            this.downloadGroupBox.Size = new System.Drawing.Size(660, 220);
            this.downloadGroupBox.TabIndex = 1;
            this.downloadGroupBox.TabStop = false;
            this.downloadGroupBox.Text = "Download";
            // 
            // downloadMissingTopStocksCheckBox
            // 
            this.downloadMissingTopStocksCheckBox.AutoSize = true;
            this.downloadMissingTopStocksCheckBox.Location = new System.Drawing.Point(9, 191);
            this.downloadMissingTopStocksCheckBox.Name = "downloadMissingTopStocksCheckBox";
            this.downloadMissingTopStocksCheckBox.Size = new System.Drawing.Size(143, 17);
            this.downloadMissingTopStocksCheckBox.TabIndex = 13;
            this.downloadMissingTopStocksCheckBox.Text = "Only Missing Top Stocks";
            this.downloadMissingTopStocksCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadCompanyQuotesCheckBox
            // 
            this.downloadCompanyQuotesCheckBox.AutoSize = true;
            this.downloadCompanyQuotesCheckBox.Checked = true;
            this.downloadCompanyQuotesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadCompanyQuotesCheckBox.Location = new System.Drawing.Point(9, 168);
            this.downloadCompanyQuotesCheckBox.Name = "downloadCompanyQuotesCheckBox";
            this.downloadCompanyQuotesCheckBox.Size = new System.Drawing.Size(60, 17);
            this.downloadCompanyQuotesCheckBox.TabIndex = 12;
            this.downloadCompanyQuotesCheckBox.Text = "Quotes";
            this.downloadCompanyQuotesCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadCompanyProfileCheckBox
            // 
            this.downloadCompanyProfileCheckBox.AutoSize = true;
            this.downloadCompanyProfileCheckBox.Checked = true;
            this.downloadCompanyProfileCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadCompanyProfileCheckBox.Location = new System.Drawing.Point(9, 145);
            this.downloadCompanyProfileCheckBox.Name = "downloadCompanyProfileCheckBox";
            this.downloadCompanyProfileCheckBox.Size = new System.Drawing.Size(55, 17);
            this.downloadCompanyProfileCheckBox.TabIndex = 11;
            this.downloadCompanyProfileCheckBox.Text = "Profile";
            this.downloadCompanyProfileCheckBox.UseVisualStyleBackColor = true;
            // 
            // topStocksNumericUpDown
            // 
            this.topStocksNumericUpDown.Location = new System.Drawing.Point(170, 69);
            this.topStocksNumericUpDown.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.topStocksNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.topStocksNumericUpDown.Name = "topStocksNumericUpDown";
            this.topStocksNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.topStocksNumericUpDown.TabIndex = 6;
            this.topStocksNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // downloadCompanyMarketQuotesCheckBox
            // 
            this.downloadCompanyMarketQuotesCheckBox.AutoSize = true;
            this.downloadCompanyMarketQuotesCheckBox.Checked = true;
            this.downloadCompanyMarketQuotesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadCompanyMarketQuotesCheckBox.Location = new System.Drawing.Point(330, 46);
            this.downloadCompanyMarketQuotesCheckBox.Name = "downloadCompanyMarketQuotesCheckBox";
            this.downloadCompanyMarketQuotesCheckBox.Size = new System.Drawing.Size(143, 17);
            this.downloadCompanyMarketQuotesCheckBox.TabIndex = 4;
            this.downloadCompanyMarketQuotesCheckBox.Text = "Company Market Quotes";
            this.downloadCompanyMarketQuotesCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadIndustryMarketQuotesCheckBox
            // 
            this.downloadIndustryMarketQuotesCheckBox.AutoSize = true;
            this.downloadIndustryMarketQuotesCheckBox.Checked = true;
            this.downloadIndustryMarketQuotesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadIndustryMarketQuotesCheckBox.Location = new System.Drawing.Point(170, 46);
            this.downloadIndustryMarketQuotesCheckBox.Name = "downloadIndustryMarketQuotesCheckBox";
            this.downloadIndustryMarketQuotesCheckBox.Size = new System.Drawing.Size(136, 17);
            this.downloadIndustryMarketQuotesCheckBox.TabIndex = 3;
            this.downloadIndustryMarketQuotesCheckBox.Text = "Industry Market Quotes";
            this.downloadIndustryMarketQuotesCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadSectorMarketQuotesCheckBox
            // 
            this.downloadSectorMarketQuotesCheckBox.AutoSize = true;
            this.downloadSectorMarketQuotesCheckBox.Checked = true;
            this.downloadSectorMarketQuotesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadSectorMarketQuotesCheckBox.Location = new System.Drawing.Point(9, 46);
            this.downloadSectorMarketQuotesCheckBox.Name = "downloadSectorMarketQuotesCheckBox";
            this.downloadSectorMarketQuotesCheckBox.Size = new System.Drawing.Size(130, 17);
            this.downloadSectorMarketQuotesCheckBox.TabIndex = 2;
            this.downloadSectorMarketQuotesCheckBox.Text = "Sector Market Quotes";
            this.downloadSectorMarketQuotesCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadCompanyIDsCheckBox
            // 
            this.downloadCompanyIDsCheckBox.AutoSize = true;
            this.downloadCompanyIDsCheckBox.Checked = true;
            this.downloadCompanyIDsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadCompanyIDsCheckBox.Location = new System.Drawing.Point(170, 23);
            this.downloadCompanyIDsCheckBox.Name = "downloadCompanyIDsCheckBox";
            this.downloadCompanyIDsCheckBox.Size = new System.Drawing.Size(89, 17);
            this.downloadCompanyIDsCheckBox.TabIndex = 1;
            this.downloadCompanyIDsCheckBox.Text = "Company IDs";
            this.downloadCompanyIDsCheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadSectorsAndIndustriesCheckBox
            // 
            this.downloadSectorsAndIndustriesCheckBox.AutoSize = true;
            this.downloadSectorsAndIndustriesCheckBox.Checked = true;
            this.downloadSectorsAndIndustriesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadSectorsAndIndustriesCheckBox.Enabled = false;
            this.downloadSectorsAndIndustriesCheckBox.Location = new System.Drawing.Point(9, 23);
            this.downloadSectorsAndIndustriesCheckBox.Name = "downloadSectorsAndIndustriesCheckBox";
            this.downloadSectorsAndIndustriesCheckBox.Size = new System.Drawing.Size(132, 17);
            this.downloadSectorsAndIndustriesCheckBox.TabIndex = 0;
            this.downloadSectorsAndIndustriesCheckBox.Text = "Sectors And Industries";
            this.downloadSectorsAndIndustriesCheckBox.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(579, 48);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 15;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(579, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 14;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // logGroupBox
            // 
            this.logGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logGroupBox.Controls.Add(this.logTextBox);
            this.logGroupBox.Location = new System.Drawing.Point(12, 394);
            this.logGroupBox.Name = "logGroupBox";
            this.logGroupBox.Size = new System.Drawing.Size(660, 177);
            this.logGroupBox.TabIndex = 2;
            this.logGroupBox.TabStop = false;
            this.logGroupBox.Text = "Log";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.BackColor = System.Drawing.Color.Black;
            this.logTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTextBox.ForeColor = System.Drawing.Color.Lime;
            this.logTextBox.Location = new System.Drawing.Point(6, 19);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(648, 152);
            this.logTextBox.TabIndex = 0;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 577);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(660, 23);
            this.progressBar.TabIndex = 3;
            // 
            // downloadTopStocksRadioButton
            // 
            this.downloadTopStocksRadioButton.AutoSize = true;
            this.downloadTopStocksRadioButton.Checked = true;
            this.downloadTopStocksRadioButton.Location = new System.Drawing.Point(9, 69);
            this.downloadTopStocksRadioButton.Name = "downloadTopStocksRadioButton";
            this.downloadTopStocksRadioButton.Size = new System.Drawing.Size(83, 17);
            this.downloadTopStocksRadioButton.TabIndex = 5;
            this.downloadTopStocksRadioButton.TabStop = true;
            this.downloadTopStocksRadioButton.Text = "Top Stocks:";
            this.downloadTopStocksRadioButton.UseVisualStyleBackColor = true;
            // 
            // downloadStocksFromTableRadioButton
            // 
            this.downloadStocksFromTableRadioButton.AutoSize = true;
            this.downloadStocksFromTableRadioButton.Location = new System.Drawing.Point(9, 96);
            this.downloadStocksFromTableRadioButton.Name = "downloadStocksFromTableRadioButton";
            this.downloadStocksFromTableRadioButton.Size = new System.Drawing.Size(117, 17);
            this.downloadStocksFromTableRadioButton.TabIndex = 7;
            this.downloadStocksFromTableRadioButton.Text = "Stocks From Table:";
            this.downloadStocksFromTableRadioButton.UseVisualStyleBackColor = true;
            // 
            // stocksTableNameTextBox
            // 
            this.stocksTableNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stocksTableNameTextBox.Location = new System.Drawing.Point(170, 95);
            this.stocksTableNameTextBox.Name = "stocksTableNameTextBox";
            this.stocksTableNameTextBox.Size = new System.Drawing.Size(484, 20);
            this.stocksTableNameTextBox.TabIndex = 8;
            this.stocksTableNameTextBox.Text = "sp500_2015";
            // 
            // downloadSelectedStocksRadioButton
            // 
            this.downloadSelectedStocksRadioButton.AutoSize = true;
            this.downloadSelectedStocksRadioButton.Location = new System.Drawing.Point(9, 122);
            this.downloadSelectedStocksRadioButton.Name = "downloadSelectedStocksRadioButton";
            this.downloadSelectedStocksRadioButton.Size = new System.Drawing.Size(106, 17);
            this.downloadSelectedStocksRadioButton.TabIndex = 9;
            this.downloadSelectedStocksRadioButton.Text = "Selected Stocks:";
            this.downloadSelectedStocksRadioButton.UseVisualStyleBackColor = true;
            // 
            // selectedStocksTextBox
            // 
            this.selectedStocksTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedStocksTextBox.Location = new System.Drawing.Point(170, 121);
            this.selectedStocksTextBox.Multiline = true;
            this.selectedStocksTextBox.Name = "selectedStocksTextBox";
            this.selectedStocksTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.selectedStocksTextBox.Size = new System.Drawing.Size(484, 87);
            this.selectedStocksTextBox.TabIndex = 10;
            this.selectedStocksTextBox.Text = "AAPL, MSFT, GOOGL, INTC, IBM";
            // 
            // YahooDownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 612);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.logGroupBox);
            this.Controls.Add(this.downloadGroupBox);
            this.Controls.Add(this.mysqlGroupBox);
            this.MinimumSize = new System.Drawing.Size(700, 650);
            this.Name = "YahooDownloaderForm";
            this.Text = "Yahoo Downloader";
            this.mysqlGroupBox.ResumeLayout(false);
            this.mysqlGroupBox.PerformLayout();
            this.downloadGroupBox.ResumeLayout(false);
            this.downloadGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topStocksNumericUpDown)).EndInit();
            this.logGroupBox.ResumeLayout(false);
            this.logGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mysqlGroupBox;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.Label hostLabel;
        private System.Windows.Forms.GroupBox downloadGroupBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox logGroupBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.CheckBox downloadSectorsAndIndustriesCheckBox;
        private System.Windows.Forms.CheckBox downloadCompanyIDsCheckBox;
        private System.Windows.Forms.CheckBox downloadSectorMarketQuotesCheckBox;
        private System.Windows.Forms.CheckBox downloadIndustryMarketQuotesCheckBox;
        private System.Windows.Forms.CheckBox downloadCompanyMarketQuotesCheckBox;
        private System.Windows.Forms.TextBox tablePrefixTextBox;
        private System.Windows.Forms.Label tablePrefixLabel;
        private System.Windows.Forms.CheckBox downloadCompanyQuotesCheckBox;
        private System.Windows.Forms.CheckBox downloadCompanyProfileCheckBox;
        private System.Windows.Forms.NumericUpDown topStocksNumericUpDown;
        private System.Windows.Forms.CheckBox downloadMissingTopStocksCheckBox;
        private System.Windows.Forms.TextBox selectedStocksTextBox;
        private System.Windows.Forms.RadioButton downloadSelectedStocksRadioButton;
        private System.Windows.Forms.TextBox stocksTableNameTextBox;
        private System.Windows.Forms.RadioButton downloadStocksFromTableRadioButton;
        private System.Windows.Forms.RadioButton downloadTopStocksRadioButton;
    }
}

