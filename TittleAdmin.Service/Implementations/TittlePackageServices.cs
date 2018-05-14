using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;

namespace TittleAdmin.Service.Implementations
{
    public class TittlePackageServices
    {
        public TittlePackageServices()
        {
        }


        #region Revenue chart
        /// <summary>
        /// Revenue chart
        /// </summary>
        /// <returns></returns>
        public List<CustomChartData<decimal>> RevenueChart(int type)
        {
            List<CustomChartData<decimal>> _data = new List<CustomChartData<decimal>>();
            List<CustomChartData<decimal>> _dataQuery = new List<CustomChartData<decimal>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                if (type == 0)
                {
                    strQuery.Append("SELECT 'Today' as xAxis");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', p.amount, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', p.amount, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) = curdate() ");
                    strQuery.Append("group by Date(up.created_at) ");
                    _data = db.Database.SqlQuery<CustomChartData<decimal>>(strQuery.ToString()).ToList();
                    if (_data == null || _data.Count == 0)
                    {
                        _data.Add(new CustomChartData<decimal>
                        {
                            xAxis = "Today",
                            yAxisAndroid = 0,
                            yAxisIphone = 0
                        });
                    }
                }
                else if (type == 1)
                {
                    strQuery.Append("SELECT DATE_FORMAT(up.created_at, '%d %b') as 'xAxis'");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', p.amount, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', p.amount, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) >= date_add(curdate(), INTERVAL -7 DAY) ");
                    strQuery.Append("group by Date(up.created_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<decimal>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 6; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<decimal>
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
                            _data.Add(new CustomChartData<decimal>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = 0,
                                yAxisIphone = 0
                            });
                        }
                    }
                }
                else
                {
                    strQuery.Append("SELECT DATE_FORMAT(up.created_at, '%d %b') as 'xAxis'");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', p.amount, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', p.amount, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) >= date_add(curdate(), INTERVAL -30 DAY) ");
                    strQuery.Append("group by Date(up.created_at) ");
                    _dataQuery = db.Database.SqlQuery<CustomChartData<decimal>>(strQuery.ToString()).ToList();
                    bool isFound = false;
                    for (int i = 29; i >= 0; i--)
                    {
                        isFound = false;
                        for (int j = 0; j < _dataQuery.Count; j++)
                        {
                            if (DateTime.Now.AddDays(-i).ToString("dd MMM") == _dataQuery[j].xAxis)
                            {
                                isFound = true;
                                _data.Add(new CustomChartData<decimal>
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
                            _data.Add(new CustomChartData<decimal>
                            {
                                xAxis = DateTime.Now.AddDays(-i).ToString("dd MMM"),
                                yAxisAndroid = 0,
                                yAxisIphone = 0
                            });
                        }
                    }
                }
            }
            return _data;
        }
        #endregion

        #region Total In App purchases chart
        /// <summary>
        /// Total In App purchases chart
        /// </summary>
        /// <returns></returns>
        public List<CustomChartData<string>> InappPurchaseChart(int type)
        {
            List<CustomChartData<string>> _data = new List<CustomChartData<string>>();
            List<CustomChartData<string>> _dataQuery = new List<CustomChartData<string>>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                if (type == 0)
                {
                    strQuery.Append("SELECT 'Today' as xAxis");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) = curdate() ");
                    strQuery.Append("group by Date(up.created_at) ");
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
                    strQuery.Append("SELECT DATE_FORMAT(up.created_at, '%d %b') as 'xAxis'");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) >= date_add(curdate(), INTERVAL -7 DAY) ");
                    strQuery.Append("group by Date(up.created_at) ");
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
                    strQuery.Append("SELECT DATE_FORMAT(up.created_at, '%d %b') as 'xAxis'");
                    strQuery.Append(", IFNULL(SUM(if(d.os like '%android%', 1, 0)),0) as yAxisAndroid ");
                    strQuery.Append(", IFNULL(SUM(if (d.os like '%ios%', 1, 0)),0) as yAxisIphone ");
                    strQuery.Append("FROM plans as p join ");
                    strQuery.Append("user_plans as up on p.id = up.plan_id join ");
                    strQuery.Append("users as u on up.user_id = u.id join ");
                    strQuery.Append("devices as d on u.id = d.deviceable_id ");
                    strQuery.Append("where Date(up.created_at) >= date_add(curdate(), INTERVAL -30 DAY) ");
                    strQuery.Append("group by Date(up.created_at) ");
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

        #region Add on feature list
        /// <summary>
        /// Add on feature list
        /// </summary>
        /// <returns></returns>
        public List<CustomAddOnData> GetAddOnFeatures()
        {
            List<CustomAddOnData> _data = new List<CustomAddOnData>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("SELECT c.PackageFullName, c.CurrentTotal, a.ActiveTotal, 0 AS ExpiredTotal FROM ");
                strQuery.Append("( ");
                strQuery.Append("SELECT PackageFullName, COUNT(t.plan_id) AS CurrentTotal FROM ");
                strQuery.Append("( ");
                strQuery.Append("SELECT CONCAT( 'Plan ', p.name) AS PackageFullName, ");
                strQuery.Append("up.plan_id, up.user_id ");
                strQuery.Append("FROM plans AS p ");
                strQuery.Append("LEFT JOIN user_plans AS up ON up.plan_id = p.id ");
                strQuery.Append("LEFT JOIN users AS u ON u.id = up.user_id ");
                strQuery.Append(") AS t ");
                strQuery.Append("GROUP BY t.PackageFullName ");
                strQuery.Append(") AS c ");
                strQuery.Append("LEFT JOIN ");
                strQuery.Append("( ");
                strQuery.Append("SELECT t.PackageFullName, ");
                strQuery.Append("COUNT(t.plan_id) AS ActiveTotal FROM ");
                strQuery.Append("( ");
                strQuery.Append("SELECT CONCAT( 'Plan ', p.name) AS PackageFullName, ");
                strQuery.Append("up.plan_id, up.user_id, ");
                strQuery.Append("TIMESTAMPDIFF(DAY, MAX(uh1.updated_at), CURDATE()) AS date_not_active ");
                strQuery.Append("FROM plans AS p ");
                strQuery.Append("LEFT JOIN user_plans AS up ON up.plan_id = p.id ");
                strQuery.Append("LEFT JOIN users AS u ON u.id = up.user_id ");
                strQuery.Append("LEFT JOIN user_histories AS uh1 ON uh1.user_id = up.user_id ");
                strQuery.Append("GROUP BY up.plan_id, up.user_id ");
                strQuery.Append(") AS t ");
                strQuery.Append("WHERE t.date_not_active < 90 OR t.PackageFullName = 'Plan A' ");
                strQuery.Append("GROUP BY t.PackageFullName ");
                strQuery.Append(") AS a ON a.PackageFullName = c.PackageFullName ");
                _data = db.Database.SqlQuery<CustomAddOnData>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion
    }
}
