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
                strQuery.Append("SELECT PackageFullName, sum(if (expired_at>now(), 1, 0)) as CurrentTotal,");
                strQuery.Append(" 0 as ActiveTotal, ");
                strQuery.Append("sum(if (expired_at <= now(), 1, 0)) as ExpiredTotal from ");
                strQuery.Append("( ");
                strQuery.Append("SELECT concat(if (p.type = 'access', 'Parent - ', 'Child - '),p.unit,' Device') as PackageFullName,  ");
                strQuery.Append("up.package_id, p.unit, p.type, if (up.expired_at = null,now(),up.expired_at) as expired_at ");
                strQuery.Append("FROM packages as p left join ");
                strQuery.Append("user_package as up on up.package_id = p.id left join ");
                strQuery.Append("users as u on u.id = up.user_id ");
                strQuery.Append("where p.version = 2 AND p.unit <> 0 ");
                strQuery.Append(") as t ");
                strQuery.Append("group by t.PackageFullName ");
                strQuery.Append("order by t.type,t.unit ");
                _data = db.Database.SqlQuery<CustomAddOnData>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion
    }
}
