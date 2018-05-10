using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;

namespace TittleAdmin.Service.Implementations
{
    public class TittleNotificationServices
    {
        #region Notifications list
        /// <summary>
        /// Notifications list
        /// </summary>
        /// <returns></returns>
        public List<CustomNotification> GetNotificationList(string searchBy, int take, int skip, string sortBy, bool sortDir,string customField, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "(name like '%" + searchBy + "%' OR OnDate like '%" + searchBy + "%' OR ";
            whereClause += "NextNotificationDate like '%" + searchBy + "%' OR content like '%" + searchBy + "%' OR ";
            whereClause += "type like '%" + searchBy + "%' OR status like '%" + searchBy + "%' OR ";
            whereClause += "data like '%" + searchBy + "%') ";
            if (customField == "Today")
            {
                whereClause += " AND type='published' AND  STR_TO_DATE(NextNotificationDate, '%d/%m/%Y %T')>=now() AND STR_TO_DATE(NextNotificationDate, '%d/%m/%Y %T')<=DATE_ADD(now(), INTERVAL 1 DAY)";
            }

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "name";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%d/%m/%Y %T')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomNotification> _data = new List<CustomNotification>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomNotification> _dataFiltered = new List<CustomNotification>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("Select id, name,OnDate,NextNotificationDate,content,type,status, ");
                strFilteredQuery.Append("if (data = 'all','All user',concat((CHAR_LENGTH(data) - CHAR_LENGTH(REPLACE(data, ',', '')) + 1), ' users')) as data ");
                strFilteredQuery.Append(" from( ");
                strFilteredQuery.Append("SELECT id, name, DATE_FORMAT(time, '%d/%m/%Y %T') as OnDate, ");
                strFilteredQuery.Append("DATE_FORMAT(next_notification, '%d/%m/%Y %T') as NextNotificationDate, ");
                strFilteredQuery.Append("content, type, status, replace(replace(data, '{\"users\":\"', ''), '\"}', '') as data ");
                strFilteredQuery.Append("FROM notifications where type<>'' AND status<>'completed') as n) as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomNotification>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region Get Notification Info
        /// <summary>
        /// Get Notification Info
        /// </summary>
        /// <returns></returns>
        public notification GetNotificationInfo(long nID)
        {
            List<notification> NotificationInfo;
            using (var db = new TittleEntities())
            {
                NotificationInfo = db.notifications.Where(x => x.id == nID).ToList();
            }
            return NotificationInfo[0];
        }
        #endregion

        #region Delete Notification Info
        /// <summary>
        /// Delete Notification
        /// </summary>
        /// <returns></returns>
        public void DeleteNotification(long nID, ref string sMessage)
        {
            sMessage = "Delete can't be completed , there are ";
            notification obj = GetNotificationInfo(nID);
            using (var db = new TittleEntities())
            {
                db.notifications.Attach(obj);
                db.notifications.Remove(obj);
                db.SaveChanges();
            }
            sMessage = "Success";
        }
        #endregion

        #region Save Or Update Notification
        /// <summary>
        /// Save or update Notification info
        /// </summary>
        /// <param name="_notification"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdateNotification(CustomNotification _notification, ref long nID)
        {
            string sMessage = "Success";

            notification obj;
            if (_notification.id != 0)
                obj = GetNotificationInfo(_notification.id);
            else
                obj = new notification();


            using (var db = new TittleEntities())
            {
                obj.content = _notification.content;
                obj.name = _notification.name;
                obj.time = DateTime.ParseExact(_notification.OnDate, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                obj.type = _notification.type;
                //obj.next_notification = DateTime.ParseExact(_notification.NextNotificationDate, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                obj.status = _notification.status;
                obj.updated_at = DateTime.Now;

                if (_notification.id != 0)
                {
                    db.notifications.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                }
                else
                {
                    obj.created_at = DateTime.Now;
                    db.notifications.Add(obj);
                }
                db.SaveChanges();
                nID = obj.id;
            }

            return sMessage;
        }
        #endregion

        #region Notification Users list
        /// <summary>
        /// Notification Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomNotificationUser> GetNotificationUsersList(long Id)
        {
            List<CustomNotificationUser> _data = new List<CustomNotificationUser>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("SELECT user.id, users.email as Email");
                strQuery.Append(" FROM notifications join");
                strQuery.Append(" users");
                strQuery.Append(" on replace(replace(data, '{\"users\":\"', ''), '\"}', '')='all' OR users.id in (replace(replace(data, '{\"users\":\"', ''), '\"}', ''))");
                strQuery.Append(" join devices on users.id=devices.deviceable_id where users.active=1");
                _data = db.Database.SqlQuery<CustomNotificationUser>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion

        #region Get System Notification Info
        /// <summary>
        /// Get System Notification Info
        /// </summary>
        /// <returns></returns>
        public system_settings GetSystemNotificationInfo()
        {
            List<system_settings> NotificationInfo;
            using (var db = new TittleEntities())
            {
                NotificationInfo = db.system_settings.Where(x => x.key == "reminder_notification").ToList();
            }
            return NotificationInfo[0];
        }
        #endregion

        #region Update System Notification Info
        /// <summary>
        /// Update System Notification Info
        /// </summary>
        /// <returns></returns>
        public void UpdateSystemNotificationInfo(string value)
        {
            using (var db = new TittleEntities())
            {
                system_settings obj = db.system_settings.Where(x => x.key == "reminder_notification").FirstOrDefault();
                obj.value = value;

                db.system_settings.Attach(obj);
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        #endregion

        #region Active Notifications list
        /// <summary>
        /// Active Notifications list
        /// </summary>
        /// <returns></returns>
        public List<notification> GetActiveNotifications()
        {
            List<notification> notifications = new List<notification>();
            DateTime dt = DateTime.Now;
            using (var db = new TittleEntities())
            {
                notifications = db.notifications.Where(x => x.status == "published" && (DateTime)x.next_notification <= dt).ToList();
            }
            return notifications;
        }
        #endregion

        #region Save notification box info
        /// <summary>
        /// Save notification box info
        /// </summary>
        /// <returns></returns>
        public void SaveNotificationBoxInfo(notification_boxes obj)
        {
            using (var db = new TittleEntities())
            {
                db.notification_boxes.Add(obj);
                db.SaveChanges();
            }
        }
        #endregion

        #region Devices list
        /// <summary>
        /// Devices list
        /// </summary>
        /// <returns></returns>
        public List<device> GetListOfDevices(long id)
        {
            List<device> devices = new List<device>();
            using (var db = new TittleEntities())
            {
                devices = db.devices.Where(x => x.device_id == id).ToList();
            }
            return devices;
        }
        #endregion

        #region Update notification info
        /// <summary>
        /// Update notification info
        /// </summary>
        /// <returns></returns>
        public void UpdateNotificationInfo(notification obj)
        {
            using (var db = new TittleEntities())
            {
                db.notifications.Add(obj);
                db.SaveChanges();
            }
        }
        #endregion

        #region Notification Users list
        /// <summary>
        /// Notification Users list
        /// </summary>
        /// <returns></returns>
        public List<CustomNotificationUser> GetUserDetailByEmail(string email)
        {
            List<CustomNotificationUser> _data = new List<CustomNotificationUser>();
            StringBuilder strQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strQuery.Append("SELECT user.id, users.email as Email");
                strQuery.Append(" FROM users ");
                strQuery.Append(" where users.active=1 AND users.email='" + email + "'");
                _data = db.Database.SqlQuery<CustomNotificationUser>(strQuery.ToString()).ToList();
            }
            return _data;
        }
        #endregion

        #region Save Or Update Notification
        /// <summary>
        /// Save or update Notification info
        /// </summary>
        /// <param name="_notification"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string AddNotification(CustomNotification _notification, ref long nID)
        {
            string sMessage = "Success";

            notification obj;
            if (_notification.id != 0)
                obj = GetNotificationInfo(_notification.id);
            else
                obj = new notification();


            using (var db = new TittleEntities())
            {
                obj.data = _notification.data;
                obj.content = _notification.content;
                obj.name = _notification.name;
                obj.time = DateTime.ParseExact(_notification.OnDate, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                obj.type = _notification.type;
                obj.next_notification = DateTime.ParseExact(_notification.NextNotificationDate, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
                obj.status = _notification.status;
                obj.updated_at = DateTime.Now;

                if (_notification.id != 0)
                {
                    db.notifications.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                }
                else
                {
                    obj.created_at = DateTime.Now;
                    db.notifications.Add(obj);
                }
                db.SaveChanges();
                nID = obj.id;
            }

            return sMessage;
        }
        #endregion
    }
}
