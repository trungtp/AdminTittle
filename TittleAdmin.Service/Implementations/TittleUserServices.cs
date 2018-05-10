using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TittleAdmin.Model;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;

namespace TittleAdmin.Service.Implementations
{
    public class TittleUserServices
    {
        public TittleUserServices()
        {
        }

        #region Validate User
        /// <summary>
        /// Validate User
        /// </summary>
        /// <returns></returns>
        public List<user> ValidateLoginInfo(string sUsername, string sPassword, ref long nId, ref string sMessage)
        {
            List<user> LoginInfo;

            using (var db = new TittleEntities())
            {
                LoginInfo = db.users.Where(x => x.email == sUsername).ToList();
            }
            if (LoginInfo.Count() == 0)
            {
                sMessage = "Incorrect Email /Password.";
            }
            else if (BCrypt.Net.BCrypt.Verify(sPassword,LoginInfo[0].password))
            {
                if (LoginInfo[0].active == 0)
                {
                    sMessage = "Your Account is not active.";
                }
                else if (LoginInfo[0].role != 1)
                {
                    sMessage = "Only administrator allowed to access this website.";
                }
                else {
                    nId = LoginInfo[0].id;
                }
            }
            else
            {
                sMessage = "Incorrect Email /Password.";
            }
            return LoginInfo;
        }
        #endregion

        #region Validate Forgot Email
        /// <summary>
        /// Validate Forgot Email
        /// </summary>
        /// <returns></returns>
        public List<user> ValidateForgotEmail(string sUsername, ref string sMessage)
        {
            List<user> LoginInfo;
            using (var db = new TittleEntities())
            {
                LoginInfo = db.users.Where(x => x.email == sUsername).ToList();
            }
            if (LoginInfo.Count() == 0)
            {
                sMessage = "Email not exist.";
            }
            else
            {
                if (LoginInfo[0].role != 1)
                {
                    sMessage = "Only administrator allowed to access this website.";
                }
                else
                {
                    Guid newKey = Guid.NewGuid();
                    //send email with link the reset link
                    MailData _mailData = new MailData();
                    _mailData.To = sUsername;
                    _mailData.Subject = "Forgot Password Request";
                    string mesge = "Hi " + LoginInfo[0].name + ",<br />";
                    mesge += "You have forgotten your password.<br />";
                    mesge += "To reset your password, please copy & paste below link on browser.<br />";
                    mesge += "http://localhost:65339/reset?key=" + newKey.ToString();
                    mesge += "<br /><br />Thanks<br />";
                    mesge += "Tittle";
                    _mailData.Body = mesge;
                    if (MailService.SendEmail(_mailData))
                    {
                        password_resets _pass_reset = new password_resets();
                        _pass_reset.created_at = DateTime.Now;
                        _pass_reset.email = sUsername;
                        _pass_reset.token = newKey.ToString();

                        sMessage = "Please check you email for reset password instructions.";
                        using (var db = new TittleEntities())
                        {
                            db.password_resets.Add(_pass_reset);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        sMessage = "Error occurred in sending email, please try again.";
                    }
                }
            }
            return LoginInfo;
        }
        #endregion

        #region Validate Reset Password key
        /// <summary>
        /// Validate Reset Password key
        /// </summary>
        /// <returns></returns>
        public CustomPasswordReset ValidateResetKey(string sToken, ref string sMessage)
        {
            CustomPasswordReset _user = new CustomPasswordReset();
            password_resets _resetPass;
            using (var db = new TittleEntities())
            {
                _resetPass = db.password_resets.Where(x => x.token == sToken).FirstOrDefault();
            }
            if (_resetPass != null)
            {
                if (_resetPass.created_at < DateTime.Now.AddHours(-24))
                {
                    sMessage = "Token is expired. Please try again.";
                }
                else
                {
                    user _userInfo;
                    using (var db = new TittleEntities())
                    {
                        _userInfo = db.users.Where(x => x.email == _resetPass.email).FirstOrDefault();
                    }
                    if (_userInfo != null)
                    {
                        _user.Email = _userInfo.email;
                        _user.UserID = _userInfo.id;
                        _user.Token = sToken;
                    }
                }
            }
            else
            {
                sMessage = "Token is expired. Please try again.";
            }
            return _user;
        }
        #endregion

        #region Reset User Password
        /// <summary>
        /// Reset User Password
        /// </summary>
        /// <returns></returns>
        public bool ResetPassword(long nId, string sPassword, string sToken)
        {
            bool isPassSet = false;
            password_resets _resetPass;
            using (var db = new TittleEntities())
            {
                _resetPass = db.password_resets.Where(x => x.token == sToken).FirstOrDefault();
            }
            if (_resetPass != null)
            {
                user UserInfo = null;
                using (var db = new TittleEntities())
                {
                    UserInfo = db.users.Where(x => x.id == nId).FirstOrDefault();
                }
                if (UserInfo != null)
                {
                    if (_resetPass.email.Trim() != UserInfo.email.Trim())
                    {
                        return false;
                    }
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(sPassword);
                    user UserColumnInfo = new user();
                    UserColumnInfo.id = UserInfo.id;
                    UserInfo.password = hashPassword;
                    using (var db = new TittleEntities())
                    {
                        try
                        {
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.users.Attach(UserInfo);
                            db.Entry(UserInfo).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            db.Configuration.ValidateOnSaveEnabled = true;
                        }
                    }
                    isPassSet = true;
                    using (var db = new TittleEntities())
                    {
                        db.password_resets.Attach(_resetPass);
                        db.password_resets.Remove(_resetPass);
                        db.SaveChanges();
                    }
                }
            }
            return isPassSet;
        }
        #endregion

        #region Get User Info
        /// <summary>
        /// Get User Info
        /// </summary>
        /// <returns></returns>
        public user GetUserInfo(long nUserID)
        {
            List<user> UserInfo;
            using (var db = new TittleEntities())
            {
                UserInfo = db.users.Where(x => x.id == nUserID).ToList();
            }
            return UserInfo[0];
        }
        #endregion

        #region Save Or Update User
        public string SaveOrUpdateUser(user _User, ref int nCustID)
        {
            string sMessage = "Success";

            user objUser;
            if (_User.id != 0)
                objUser = GetUserInfo(_User.id);
            else
                objUser = new user();


            //if (_User.id != 0)
            //{
            //    _UserRepository.Update(objUser);
            //}
            //else
            //{
            //    _UserRepository.Add(objUser);
            //}

            //unitOfWork.Commit();
            return sMessage;
        }
        #endregion

        #region Delete User Info
        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        public void DeleteAdmin(long nUserID, ref string sMessage)
        {
            sMessage = "Delete can't be completed , there are ";
            bool IsDelete = true;
            if (IsDelete)
            {
                user objAdmin = GetUserInfo(nUserID);
                //_UserRepository.Delete(x => x.id == nUserID);
                //unitOfWork.Commit();
                sMessage = "Success";
            }
        }
        #endregion

        #region Total Users Count
        /// <summary>
        /// Total Users Count
        /// </summary>
        /// <returns></returns>
        public CustomChartData<string> TotalUsersCount()
        {
            List<CustomChartData<string>> _data = new List<CustomChartData<string>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("SELECT 'Today' as xAxis");
                strQuery.Append(", SUM(if (d.os like '%android%', 1, 0)) as yAxisAndroid ");
                strQuery.Append(", SUM(if (d.os like '%ios%', 1, 0)) as yAxisIphone ");
                strQuery.Append("FROM users as u ");
                strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strQuery.Append("where Date(u.created_at) <= curdate() AND d.device_type like '%User%' ");
                _data = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
            }
            return _data[0];
        }
        #endregion

        #region New Users Count
        /// <summary>
        /// New Users Count
        /// </summary>
        /// <param name="type">0-Today,1-Week,2-Month</param>
        /// <returns></returns>
        public List<CustomChartData<string>> NewUsersCount(int type)
        {
            List<CustomChartData<string>> _data = new List<CustomChartData<string>>();
            List<CustomChartData<string>> _dataQuery = new List<CustomChartData<string>>();
            StringBuilder strQuery = new StringBuilder();

            using (var db = new TittleEntities())
            {
                if (type == 0)
                {
                    strQuery.Append("SELECT 'Today' as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(u.created_at) = curdate() AND d.device_type like '%User%' ");
                    _data = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    if (_data == null || _data.Count == 0)
                    {
                        _data.Add(new CustomChartData<string>
                        {
                            xAxis = "Today",
                            yAxisAndroid = "0",
                            yAxisIphone = "0"
                        });
                    }
                }
                else if (type == 1)
                {
                    strQuery.Append("SELECT DATE_FORMAT(u.created_at, '%d %b')  as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(u.created_at) >= date_add(curdate(),INTERVAL -7 DAY) AND d.device_type like '%User%' ");
                    strQuery.Append("group by Date(u.created_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 6; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
                else
                {
                    strQuery.Append("SELECT DATE_FORMAT(u.created_at, '%d %b')  as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(u.created_at) >= date_add(curdate(),INTERVAL -30 DAY) AND d.device_type like '%User%' ");
                    strQuery.Append("group by Date(u.created_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 29; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
            }
            return _data;
        }
        #endregion

        #region Active Users Count
        /// <summary>
        /// Active Users Count
        /// </summary>
        /// <param name="type">0-Today,1-Week,2-Month</param>
        /// <returns></returns>
        public List<CustomChartData<string>> ActiveUsersCount(int type)
        {
            List<CustomChartData<string>> _data = new List<CustomChartData<string>>();
            List<CustomChartData<string>> _dataQuery = new List<CustomChartData<string>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                if (type == 0)
                {
                    strQuery.Append("select xAxis, ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ,  ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("from (select distinct 'Today' as xAxis, user_id ");
                    strQuery.Append("from user_histories as h ");
                    strQuery.Append("where Date(h.created_at) >= curdate() ");
                    strQuery.Append(") as t join ");
                    strQuery.Append("devices as d on t.user_id = d.deviceable_id ");
                    strQuery.Append("where d.device_type like '%User%' ");
                    strQuery.Append("group by t.xAxis ");
                    _data = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    if (_data == null || _data.Count == 0)
                    {
                        _data.Add(new CustomChartData<string>
                        {
                            xAxis = "Today",
                            yAxisAndroid = "0",
                            yAxisIphone = "0"
                        });
                    }
                }
                else if (type == 1)
                {
                    strQuery.Append("select xAxis, ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ,  ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("from(");
                    strQuery.Append("SELECT DISTINCT DATE_FORMAT(h.created_at, '%d %b')  as xAxis, user_id ");
                    strQuery.Append("from user_histories as h ");
                    strQuery.Append("where Date(h.created_at) >= date_add(curdate(), INTERVAL -7 DAY) ");
                    strQuery.Append(") as t join ");
                    strQuery.Append("devices as d on t.user_id = d.deviceable_id ");
                    strQuery.Append("where d.device_type like '%User%' ");
                    strQuery.Append("group by t.xAxis ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 6; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
                else
                {
                    strQuery.Append("select xAxis, ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ,  ");
                    strQuery.Append("IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("from(");
                    strQuery.Append("SELECT DISTINCT DATE_FORMAT(h.created_at, '%d %b')  as xAxis, user_id ");
                    strQuery.Append("from user_histories as h ");
                    strQuery.Append("where Date(h.created_at) >= date_add(curdate(), INTERVAL -30 DAY) ");
                    strQuery.Append(") as t join ");
                    strQuery.Append("devices as d on t.user_id = d.deviceable_id ");
                    strQuery.Append("where d.device_type like '%User%' ");
                    strQuery.Append("group by t.xAxis ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 29; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
            }
            return _data;
        }
        #endregion

        #region Deactive Users Count
        /// <summary>
        /// Deactive Users Count
        /// </summary>
        /// <param name="type">0-Today,1-Week,2-Month</param>
        /// <returns></returns>
        public List<CustomChartData<string>> DeactiveUsersCount(int type)
        {
            List<CustomChartData<string>> _data = new List<CustomChartData<string>>();
            List<CustomChartData<string>> _dataQuery = new List<CustomChartData<string>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                if (type == 0)
                {
                    strQuery.Append("SELECT 'Today' as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where u.active=2 AND Date(u.updated_at) = curdate() AND d.device_type like '%User%' ");
                    strQuery.Append("group by Date(u.updated_at) ");
                    _data = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    if (_data == null || _data.Count == 0)
                    {
                        _data.Add(new CustomChartData<string>
                        {
                            xAxis = "Today",
                            yAxisAndroid = "0",
                            yAxisIphone = "0"
                        });
                    }
                }
                else if (type == 1)
                {
                    strQuery.Append("SELECT DATE_FORMAT(u.updated_at, '%d %b')  as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where u.active=2 AND Date(u.updated_at) >= date_add(curdate(),INTERVAL -7 DAY) AND d.device_type like '%User%' ");
                    strQuery.Append("group by Date(u.updated_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 6; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
                else
                {
                    strQuery.Append("SELECT DATE_FORMAT(u.updated_at, '%d %b')  as xAxis");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM users as u ");
                    strQuery.Append("join devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where u.active=2 AND Date(u.updated_at) >= date_add(curdate(),INTERVAL -30 DAY) AND d.device_type like '%User%' ");
                    strQuery.Append("group by Date(u.updated_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<string>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 29; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<string>
                                {
                                    xAxis = _dataQuery[j].xAxis,
                                    yAxisAndroid = _dataQuery[j].yAxisAndroid,
                                    yAxisIphone = _dataQuery[j].yAxisIphone
                                });
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            _data.Add(new CustomChartData<string>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = "0",
                                yAxisIphone = "0"
                            });
                        }
                    }
                }
            }
            return _data;
        }
        #endregion

        #region Users list
        /// <summary>
        /// Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetUsersList(string searchBy, List<string> filters, DateTime? fromDate, DateTime? toDate, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {  
            var whereClause = "(FirstName like '%"+ searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%')";

            //for Account Status
            if (filters != null)
            {
                if (filters.Contains("Activated") && filters.Contains("Yet to activate"))
                    whereClause += " AND (AccountStatus like 'Activated' OR AccountStatus like 'Yet to activate')";
                else if (filters.Contains("Activated"))
                    whereClause += " AND AccountStatus like 'Activated'";
                else if (filters.Contains("Yet to activate"))
                    whereClause += " AND AccountStatus like 'Yet to activate'";
                //for User Type
                if (filters.Contains("Active") && filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active") && filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive')";
                else if (filters.Contains("Active") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Terminated')";
                else if (filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active"))
                    whereClause += " AND (UserType like 'Active')";
                else if (filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Terminated')";
                else if (filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Inactive')";
                //for OS version
                if (filters.Contains("Android") && filters.Contains("IOS"))
                    whereClause += " AND (OS like 'Android' OR OS like 'IOS')";
                else if (filters.Contains("Android"))
                    whereClause += " AND OS like 'Android'";
                else if (filters.Contains("IOS"))
                    whereClause += " AND OS like 'IOS'";
                //for Add on purchases
                if (filters.Contains("Yes") && filters.Contains("No"))
                    whereClause += " AND (AddonPurchases like 'true' OR AddonPurchases like 'false')";
                else if (filters.Contains("Yes"))
                    whereClause += " AND AddonPurchases like 'true'";
                else if (filters.Contains("No"))
                    whereClause += " AND AddonPurchases like 'false'";
                //for Account Type
                if (filters.Contains("Facebook") && !filters.Contains("Email"))
                    whereClause += " AND (FacebookID IS NOT NULL AND FacebookID <>'')";
            }

            var timeFilter = "1=1";
            if (fromDate.HasValue)
            {
                timeFilter += " AND u.created_at >= STR_TO_DATE('" + fromDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
                timeFilter += " AND ";
                timeFilter += "u.created_at <= STR_TO_DATE('" + toDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
            }

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from ( ");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%' AND "+ timeFilter + ") as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by "+ sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Users By Country list
        /// <summary>
        /// Users By Country list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetUsersByCountryList(string searchBy, List<string> filters, DateTime? fromDate, DateTime? toDate, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "(FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%')";

            if (filters != null)
            {
                if (filters.Contains("Activated") && filters.Contains("Yet to activate"))
                    whereClause += " AND (AccountStatus like 'Activated' OR AccountStatus like 'Yet to activate')";
                else if (filters.Contains("Activated"))
                    whereClause += " AND AccountStatus like 'Activated'";
                else if (filters.Contains("Yet to activate"))
                    whereClause += " AND AccountStatus like 'Yet to activate'";
                //for User Type
                if (filters.Contains("Active") && filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active") && filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive')";
                else if (filters.Contains("Active") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Terminated')";
                else if (filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active"))
                    whereClause += " AND (UserType like 'Active')";
                else if (filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Terminated')";
                else if (filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Inactive')";
                //for OS version
                if (filters.Contains("Android") && filters.Contains("IOS"))
                    whereClause += " AND (OS like 'Android' OR OS like 'IOS')";
                else if (filters.Contains("Android"))
                    whereClause += " AND OS like 'Android'";
                else if (filters.Contains("IOS"))
                    whereClause += " AND OS like 'IOS'";
                //for Add on purchases
                if (filters.Contains("Yes") && filters.Contains("No"))
                    whereClause += " AND (AddonPurchases like 'true' OR AddonPurchases like 'false')";
                else if (filters.Contains("Yes"))
                    whereClause += " AND AddonPurchases like 'true'";
                else if (filters.Contains("No"))
                    whereClause += " AND AddonPurchases like 'false'";
                //for Account Type
                if (filters.Contains("Facebook") && !filters.Contains("Email"))
                    whereClause += " AND (FacebookID IS NOT NULL AND FacebookID <>'')";
            }

            var timeFilter = "1=1";
            if (fromDate.HasValue)
            {
                timeFilter += " AND u.created_at >= STR_TO_DATE('" + fromDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
                timeFilter += " AND ";
                timeFilter += "u.created_at <= STR_TO_DATE('" + toDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
            }

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%' AND u.country<>'' AND " + timeFilter + ") as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Perk Tasks list
        /// <summary>
        /// Perk Tasks list
        /// </summary>
        /// <returns></returns>
        public List<CustomPerkTask> GetPerkTasks(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "perks_tasks.key like '%" + searchBy + "%' OR perks_tasks.name like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "id";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomPerkTask> _data = new List<CustomPerkTask>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomPerkTask> _dataFiltered = new List<CustomPerkTask>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select id,perks_tasks.key,perks_tasks.name,icon,number_to_finish,score,");
                strFilteredQuery.Append("DATE_FORMAT(updated_at, '%Y-%m-%d %T') as updated_at, DATE_FORMAT(created_at, '%Y-%m-%d %T') as created_at from perks_tasks ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomPerkTask>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region Get Perk Task Info
        /// <summary>
        /// Get Perk Task Info
        /// </summary>
        /// <returns></returns>
        public perks_tasks GetPerkTaskInfo(long nID)
        {
            List<perks_tasks> PerkTasksInfo;
            using (var db = new TittleEntities())
            {
                PerkTasksInfo = db.perks_tasks.Where(x => x.id == nID).ToList();
            }
            return PerkTasksInfo[0];
        }
        #endregion

        #region GetPerkTaskByKey
        /// <summary>
        /// GetPerkTaskByKey
        /// </summary>
        /// <returns></returns>
        public perks_tasks GetPerkTaskByKey(string key)
        {
            List<perks_tasks> PerkTasksInfo;
            using (var db = new TittleEntities())
            {
                PerkTasksInfo = db.perks_tasks.Where(x => x.key == key).ToList();
            }
            if (PerkTasksInfo.Count > 0)
                return PerkTasksInfo[0];
            else
                return null;
        }
        #endregion

        #region Delete Perk Task Info
        /// <summary>
        /// Delete Perk Task
        /// </summary>
        /// <returns></returns>
        public void DeletePerkTask(long nID, ref string sMessage)
        {
            sMessage = "Delete can't be completed , there are ";
            perks_tasks obj = GetPerkTaskInfo(nID);
            using (var db = new TittleEntities())
            {
                db.perks_tasks.Attach(obj);
                db.perks_tasks.Remove(obj);
                db.SaveChanges();
            }
            sMessage = "Success";
        }
        #endregion

        #region Save Or Update Perk Task
        /// <summary>
        /// Save or update Perk Task info
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdatePerkTask(CustomPerkTask _data, ref long nID)
        {
            string sMessage = "Success";

            perks_tasks obj;
            if (_data.id != 0)
                obj = GetPerkTaskInfo(_data.id);
            else
                obj = new perks_tasks();


            using (var db = new TittleEntities())
            {
                obj.key = _data.key;
                obj.name = _data.name;
                obj.number_to_finish = _data.number_to_finish;
                obj.score = _data.score;
                obj.icon = _data.icon;
                obj.updated_at = DateTime.Now;

                if (_data.id != 0)
                {
                    db.perks_tasks.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                }
                else
                {
                    obj.created_at = DateTime.Now;
                    db.perks_tasks.Add(obj);
                }
                db.SaveChanges();
                nID = obj.id;
            }

            return sMessage;
        }
        #endregion

        #region User Plans list
        /// <summary>
        /// User Plans list
        /// </summary>
        /// <returns></returns>
        public List<CustomUserPlan> GetUserPlans(string searchByEmail, string seachByPC, string searchByPlan, string fromDate, string toDate, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "Email like '%" + searchByEmail + "%' OR PromoCode like '%" + seachByPC + "%' OR Plan like '%" + searchByPlan + "%' ";
            var timeFilter = "";
            if (!string.IsNullOrEmpty(fromDate))
                timeFilter += "up.created_at >= STR_TO_DATE(" + fromDate + ", '%Y-%m-%d %T') ";
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                timeFilter += " AND ";
                if (!string.IsNullOrEmpty(toDate))
                timeFilter += "up.created_at <= STR_TO_DATE(" + toDate + ", '%Y-%m-%d %T')";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "id";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUserPlan> _data = new List<CustomUserPlan>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUserPlan> _dataFiltered = new List<CustomUserPlan>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select *,DATE_FORMAT(tbl.created_at, '%Y-%m-%d %T') as StartDate from (");
                strFilteredQuery.Append("SELECT u.name as Name, u.email as Email,if (u.active = 1,'Activated','Yet to activate') as Status, ");
                strFilteredQuery.Append("p.name as Plan, pc.code as PromoCode, up.created_at from ");
                strFilteredQuery.Append(" users as u join ");
                strFilteredQuery.Append("user_plans as up on u.id = up.user_id join ");
                strFilteredQuery.Append("plans as p on up.plan_id = p.id left join ");
                strFilteredQuery.Append("promo_codes as pc on up.promocode_id = pc.id ");
                if(!string.IsNullOrEmpty(timeFilter)) strFilteredQuery.Append(" where "+ timeFilter);
                strFilteredQuery.Append(" ) as tbl where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUserPlan>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region User Actions list
        /// <summary>
        /// User Actions list
        /// </summary>
        /// <returns></returns>
        public List<CustomUserAction> GetUserActions(string searchBy, string searchByAction, string fromDate, string toDate, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "Email like '%" + searchBy + "%' OR Action like '%" + searchByAction + "%' ";
            var timeFilter = "";
            if (!string.IsNullOrEmpty(fromDate))
                timeFilter += "up.created_at >= STR_TO_DATE(" + fromDate + ", '%Y-%m-%d %T') ";
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                timeFilter += " AND ";
            if (!string.IsNullOrEmpty(toDate))
                timeFilter += "up.created_at <= STR_TO_DATE(" + toDate + ", '%Y-%m-%d %T')";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "id";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUserAction> _data = new List<CustomUserAction>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUserAction> _dataFiltered = new List<CustomUserAction>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select *,DATE_FORMAT(tbl.created_at, '%Y-%m-%d %T') as StartDate from (");
                strFilteredQuery.Append("SELECT u.name as Name, u.email as Email,");
                strFilteredQuery.Append("uh.action as Action, uh.created_at from ");
                strFilteredQuery.Append(" users as u join ");
                strFilteredQuery.Append("user_histories as uh on u.id = uh.user_id ");
                if (!string.IsNullOrEmpty(timeFilter)) strFilteredQuery.Append(" where " + timeFilter);
                strFilteredQuery.Append(" ) as tbl where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUserAction>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region User Actions Count
        /// <summary>
        /// User Actions Count
        /// </summary>
        /// <returns></returns>
        public List<CustomChartData<int>> UserActionsCount()
        {
            List<CustomChartData<int>> _data = new List<CustomChartData<int>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("select user_histories.action as xAxis,Count(*) as yAxisAndroid ");
                strQuery.Append("from user_histories ");
                strQuery.Append("group by user_histories.action ");
                _data = db.Database.SqlQuery<CustomChartData<int>>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion

        #region User PromoCode Count
        /// <summary>
        /// User PromoCode Count
        /// </summary>
        /// <returns></returns>
        public List<CustomChartData<int>> UserPromoCodesCount()
        {
            List<CustomChartData<int>> _data = new List<CustomChartData<int>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("select user_promo_code.promo_code_id,promo_codes.code as xAxis,Count(*) as yAxisAndroid ");
                strQuery.Append("from promo_codes join ");
                strQuery.Append("user_promo_code on promo_codes.id = user_promo_code.promo_code_id ");
                strQuery.Append("group by user_promo_code.promo_code_id ");
                _data = db.Database.SqlQuery<CustomChartData<int>>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion

        #region User Plans Count
        /// <summary>
        /// User Plans Count
        /// </summary>
        /// <returns></returns>
        public List<CustomChartData<int>> UserPlansCount()
        {
            List<CustomChartData<int>> _data = new List<CustomChartData<int>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("select user_plans.plan_id,plans.name as xAxis,Count(*) as yAxisAndroid ");
                strQuery.Append("from plans left join ");
                strQuery.Append("user_plans on plans.id = user_plans.plan_id ");
                strQuery.Append("group by user_plans.plan_id ");
                _data = db.Database.SqlQuery<CustomChartData<int>>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion

        #region Active Users list
        /// <summary>
        /// Active Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetActiveUsersList(string searchBy, List<string> filters, DateTime? fromDate, DateTime? toDate, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "(FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%')";

            //for Account Status
            if (filters != null)
            {
                if (filters.Contains("Activated") && filters.Contains("Yet to activate"))
                    whereClause += " AND (AccountStatus like 'Activated' OR AccountStatus like 'Yet to activate')";
                else if (filters.Contains("Activated"))
                    whereClause += " AND AccountStatus like 'Activated'";
                else if (filters.Contains("Yet to activate"))
                    whereClause += " AND AccountStatus like 'Yet to activate'";
                //for User Type
                if (filters.Contains("Active") && filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active") && filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Inactive')";
                else if (filters.Contains("Active") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Active' OR UserType like 'Terminated')";
                else if (filters.Contains("Inactive") && filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Inactive' OR UserType like 'Terminated')";
                else if (filters.Contains("Active"))
                    whereClause += " AND (UserType like 'Active')";
                else if (filters.Contains("Terminated"))
                    whereClause += " AND (UserType like 'Terminated')";
                else if (filters.Contains("Inactive"))
                    whereClause += " AND (UserType like 'Inactive')";
                //for OS version
                if (filters.Contains("Android") && filters.Contains("IOS"))
                    whereClause += " AND (OS like 'Android' OR OS like 'IOS')";
                else if (filters.Contains("Android"))
                    whereClause += " AND OS like 'Android'";
                else if (filters.Contains("IOS"))
                    whereClause += " AND OS like 'IOS'";
                //for Add on purchases
                if (filters.Contains("Yes") && filters.Contains("No"))
                    whereClause += " AND (AddonPurchases like 'true' OR AddonPurchases like 'false')";
                else if (filters.Contains("Yes"))
                    whereClause += " AND AddonPurchases like 'true'";
                else if (filters.Contains("No"))
                    whereClause += " AND AddonPurchases like 'false'";
                //for Account Type
                if (filters.Contains("Facebook") && !filters.Contains("Email"))
                    whereClause += " AND (FacebookID IS NOT NULL AND FacebookID <>'')";
            }

            var timeFilter = "1=1";
            if (fromDate.HasValue)
            {
                timeFilter += " AND u.created_at >= STR_TO_DATE('" + fromDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
                timeFilter += " AND ";
                timeFilter += "u.created_at <= STR_TO_DATE('" + toDate.Value.ToString("yyyy-MM-dd HH':'mm':'ss") + "', '%Y-%m-%d %T')";
            }
            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%' AND " + timeFilter + ") as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region In App Purchase Users list
        /// <summary>
        /// In App Purchase Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetInAppPurchaseUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Gross Amount Users list
        /// <summary>
        /// Gross Amount Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetGrossAmountUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Total Amount Users list
        /// <summary>
        /// Total Amount Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetTotalAmountUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Promo Code Used by Users list
        /// <summary>
        /// Promo Code Used by Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetPromoUsedUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_promo_code as upc on u.id = upc.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Perks Used by Users list
        /// <summary>
        /// Perks Used by Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetPerksUsedUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join perks_points as upc on u.id = upc.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Redeem Used by Users list
        /// <summary>
        /// Redeem Used by Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomUser> GetRedeemUsedUsersList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, out int iosCount, out int androidCount, out string countryGrouping)
        {
            var whereClause = "FirstName like '%" + searchBy + "%' OR LastName like '%" + searchBy + "%' OR ";
            whereClause += "Country like '%" + searchBy + "%' OR Phone like '%" + searchBy + "%' OR ";
            whereClause += "Email like '%" + searchBy + "%' OR FacebookID like '%" + searchBy + "%' OR ";
            whereClause += "AccountStatus like '%" + searchBy + "%' OR UserType like '%" + searchBy + "%' OR ";
            whereClause += "LastActiveDate like '%" + searchBy + "%' OR RegistrationDate like '%" + searchBy + "%' OR ";
            whereClause += "AddonPurchases like '%" + searchBy + "%' OR OS like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "FirstName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%Y-%m-%d %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUser> _data = new List<CustomUser>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUser> _dataFiltered = new List<CustomUser>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT distinct u.id,u.first_name as FirstName, u.last_name as LastName, u.country as Country, ");
                strFilteredQuery.Append("u.phone as Phone, u.email as Email, u.id_facebook as FacebookID, d.os as OS, ");
                strFilteredQuery.Append("if (u.active = 1,'Activated','Yet to activate') as AccountStatus,  ");
                strFilteredQuery.Append("if (u.active = 2,'Terminated', if (uh.id IS NOT NULL,'Active','Inactive')) as UserType,  ");
                strFilteredQuery.Append("DATE_FORMAT(u.updated_at, '%Y-%m-%d %T') as LastActiveDate, DATE_FORMAT(u.created_at, '%Y-%m-%d %T') as RegistrationDate,  ");
                strFilteredQuery.Append("if (up.id IS NOT NULL,'true','false') as AddonPurchases ");
                strFilteredQuery.Append("from users as u ");
                strFilteredQuery.Append("join devices as d on u.id = d.deviceable_id ");
                strFilteredQuery.Append("join user_redeem as ur on u.id = ur.user_id ");
                strFilteredQuery.Append("left join user_plans as up on u.id = up.user_id ");
                strFilteredQuery.Append("left join user_histories as uh on u.id = uh.user_id ");
                strFilteredQuery.Append("where d.device_type like '%User%') as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUser>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
                iosCount = _data.Count(x => x.OS == "ios");
                androidCount = _data.Count(x => x.OS == "android");
                countryGrouping = String.Join(", ", _data.GroupBy(x => x.Country.Trim()).Select(x => x.Key + " : " + x.Count().ToString()).ToList().ToArray());
            }
            return _dataFiltered;
        }
        #endregion

        #region Get User Detail
        /// <summary>
        /// Get User Detail
        /// </summary>
        /// <returns></returns>
        public CustomResetUser GetUserDetail(int id)
        {
            List<CustomResetUser> _data = new List<CustomResetUser>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("select id,active,updated_at ");
                strQuery.Append("from users where users.id="+ id.ToString());
                _data = db.Database.SqlQuery<CustomResetUser>(strQuery.ToString()).ToList();
            }
            return _data[0];
        }
        #endregion

        #region ChangeUserActiveStatus
        /// <summary>
        /// ChangeUserActiveStatus
        /// </summary>
        /// <returns></returns>
        public void ChangeUserActiveStatus(CustomResetUser model)
        {
            using (var db = new TittleEntities())
            {
                try
                {
                    user obj = db.users.Where(x => x.id == model.id).FirstOrDefault();
                    obj.active = model.active;
                    obj.updated_at = DateTime.Now;
                    db.users.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                    }
                    throw;
                }
            }
        }
        #endregion

        #region GetUserInformations
        /// <summary>
        /// GetUserInformations
        /// </summary>
        /// <returns></returns>
        public CustomUserInfo GetUserInformations(long id)
        {
            CustomUserInfo data = new CustomUserInfo();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {

                data.id = id;
                strQuery.Append("select plans.name ");
                strQuery.Append("from user_plans join plans on user_plans.plan_id=plans.id where user_plans.user_id=" + id.ToString());
                data.plans = db.Database.SqlQuery<string>(strQuery.ToString()).ToList();

                strQuery.Clear();
                strQuery.Append("select *,if(NumberInProgress>0,'Start','Not Start') as Status from ( select perks_tasks.id,perks_tasks.name as Name, ");
                strQuery.Append("IFNULL(SUM(perks_points.number_progress), 0) as NumberInProgress,");
                strQuery.Append("IFNULL(SUM(perks_points.points_reward), 0) as Points  from perks_tasks");
                strQuery.Append(" left join perks_points on perks_points.perks_task_id = perks_tasks.id AND perks_points.user_id ="+ id.ToString());
                strQuery.Append(" group by perks_tasks.id) as tbl");
                data.tasks = db.Database.SqlQuery<CustomUserTask>(strQuery.ToString()).ToList();
            }
            return data;
        }
        #endregion

        #region GetAllUsersEmail
        /// <summary>
        /// GetAllUsersEmail
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllUsersEmail()
        {
            List<String> data = new List<String>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("select distinct email from users");
                data = db.Database.SqlQuery<String>(strQuery.ToString()).ToList();
            }
            return data;
        }
        #endregion

    }
}
