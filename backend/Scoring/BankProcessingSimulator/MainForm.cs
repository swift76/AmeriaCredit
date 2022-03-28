using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Dapper;

namespace BankProcessingSimulator
{
    public partial class MainForm : Form
    {
        string ConnectionString;

        public MainForm()
        {
            InitializeComponent();
            ConnectionString = ConfigurationManager.ConnectionStrings["ScoringDB"].ConnectionString;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonApprove_Click(object sender, EventArgs e)
        {
            SetStatus(5);
        }

        private void buttonRefuse_Click(object sender, EventArgs e)
        {
            SetStatus(6);
        }

        private void SetStatus(int status)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                IEnumerable<Guid> ids = connection.Query<Guid>("select ID from Common.APPLICATION where STATUS=" + (status == 21 ? 15 : 1), commandType: CommandType.Text);
                foreach (Guid id in ids)
                {
                    switch (status)
                    {
                        case 5:
                            connection.Execute("insert into Common.APPLICATION_SCORING_RESULT (APPLICATION_ID,SCORING_NUMBER,AMOUNT,INTEREST) values ('" + id.ToString() + "', 0, 300000, 15)", commandType: CommandType.Text);
                            if (checkHasMultipleOptions.Checked)
                            {
                                connection.Execute("insert into Common.APPLICATION_SCORING_RESULT (APPLICATION_ID,SCORING_NUMBER,AMOUNT,INTEREST) values ('" + id.ToString() + "', 1, 200000, 18)", commandType: CommandType.Text);
                            }
                            connection.Execute("update Common.APPLICATION set STATUS=5,CLIENT_CODE='" + textClientCode.Text + "',HAS_BANK_CARD=" + (checkHasCards.Checked ? "1" : "0") + ",IS_DATA_COMPLETE=" + (checkIsDataComplete.Checked ? "1" : "0") + " where ID='" + id.ToString() + "'", commandType: CommandType.Text);

                            bool isRefinancing = connection.QueryFirstOrDefault<bool>("select IS_REFINANCING from Common.APPLICATION where ID='" + id.ToString() + "'", commandType: CommandType.Text);
                            if (isRefinancing)
                            {
                                connection.Execute("insert into GL.REFINANCING_LOAN (APPLICATION_ID, ROW_ID, ORIGINAL_BANK_NAME, LOAN_TYPE, INITIAL_INTEREST, CURRENCY, INITIAL_AMOUNT, CURRENT_BALANCE, DRAWDOWN_DATE, MATURITY_DATE) values ('"
                                    + id.ToString() + "', 1, N'Կոնվերս Բանկ', N'Սպառողական', 18, 'AMD', 300000, 250000, '2010-04-07', '2020-04-07')", commandType: CommandType.Text);
                                connection.Execute("insert into GL.REFINANCING_LOAN (APPLICATION_ID, ROW_ID, ORIGINAL_BANK_NAME, LOAN_TYPE, INITIAL_INTEREST, CURRENCY, INITIAL_AMOUNT, CURRENT_BALANCE, DRAWDOWN_DATE, MATURITY_DATE) values ('"
                                    + id.ToString() + "', 2, N'ՀայԷկոնոմ Բանկ', N'Օվերդրաֆտ', 16, 'USD', 150000, 100000, '2015-12-20', '2022-12-20')", commandType: CommandType.Text);
                                connection.Execute("insert into GL.REFINANCING_LOAN (APPLICATION_ID, ROW_ID, ORIGINAL_BANK_NAME, LOAN_TYPE, INITIAL_INTEREST, CURRENCY, INITIAL_AMOUNT, CURRENT_BALANCE, DRAWDOWN_DATE, MATURITY_DATE) values ('"
                                    + id.ToString() + "', 3, N'Հայբիզնես Բանկ', N'Օվերդրաֆտ', 21, 'EUR', 100000, 18000, '2017-03-03', '2022-03-03')", commandType: CommandType.Text);
                            }
                            break;
                        case 6:
                            connection.Execute("update Common.APPLICATION set REFUSAL_REASON=N'Անբավարար միջոցներ',STATUS=6 where ID='" + id.ToString() + "'", commandType: CommandType.Text);
                            break;
                        case 7:
                            connection.Execute("update Common.APPLICATION set MANUAL_REASON=N'Վերստուգման կարիք կա',STATUS=7 where ID='" + id.ToString() + "'", commandType: CommandType.Text);
                            break;
                        case 21:
                            connection.Execute("update Common.APPLICATION set STATUS=21 where ID='" + id.ToString() + "'", commandType: CommandType.Text);
                            break;
                    }
                }
            }
        }

        private void buttonManual_Click(object sender, EventArgs e)
        {
            SetStatus(7);
        }

        private void buttonGive_Click(object sender, EventArgs e)
        {
            SetStatus(21);
        }
    }
}
