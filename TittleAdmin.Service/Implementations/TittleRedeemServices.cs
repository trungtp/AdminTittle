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
    public class TittleRedeemServices
    {
        #region User Redeem list
        /// <summary>
        /// User Redeem list
        /// </summary>
        /// <returns></returns>
        public List<CustomUserRedeem> GetUserRedeemList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "UserName like '%" + searchBy + "%' OR Redeem like '%" + searchBy + "%' OR ";
            whereClause += "DateRedeem like '%" + searchBy + "%' OR Status like '%" + searchBy + "%' OR ";
            whereClause += "ActionName like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "UserName";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%d/%m/%Y')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomUserRedeem> _data = new List<CustomUserRedeem>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomUserRedeem> _dataFiltered = new List<CustomUserRedeem>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT ur.id, DATE_FORMAT(ur.date_redeem, '%d/%m/%Y') as DateRedeem, ");
                strFilteredQuery.Append("if (ur.status = 0,'Ordered',if (ur.status = 1,'Approved','Completed')) as Status, ");
                strFilteredQuery.Append("if (isnull(u.id),'no name',u.name) as UserName, ");
                strFilteredQuery.Append("if (isnull(r.id),'no redeem',r.name) as Redeem, ");
                strFilteredQuery.Append("if (ur.status = 0,'Approved',if (ur.status = 1,'Completed','')) as ActionName ");
                strFilteredQuery.Append(" FROM user_redeem as ur left join ");
                strFilteredQuery.Append(" users as u on ur.user_id = u.id left join ");
                strFilteredQuery.Append(" redeem as r on ur.redeem_id = r.id) as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomUserRedeem>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion
        #region Get Redeem Info
        /// <summary>
        /// Get Redeem Info
        /// </summary>
        /// <returns></returns>
        public user_redeem GetRedeemInfo(long nID)
        {
            List<user_redeem> RedeemInfo;
            using (var db = new TittleEntities())
            {
                RedeemInfo = db.user_redeem.Where(x => x.id == nID).ToList();
            }
            return RedeemInfo[0];
        }
        #endregion
        #region Change Redeem Status
        /// <summary>
        /// Change Redeem Status
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="sStatus"></param>
        /// <returns></returns>
        public string ChangeRedeemStatus(long nID, string sStatus)
        {
            string sMessage = "Success";

            user_redeem obj = GetRedeemInfo(nID);

            using (var db = new TittleEntities())
            {
                if (sStatus == "Approved")
                    obj.status = 1;
                else if (sStatus == "Completed")
                    obj.status = 2;
                else
                    obj.status = 0;

                db.user_redeem.Attach(obj);
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }

            return sMessage;
        }
        #endregion

        #region Redemption Gifts list
        /// <summary>
        /// Redemption Gifts list
        /// </summary>
        /// <returns></returns>
        public List<CustomRedemptionGift> GetRedemptionGifts(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "name like '%" + searchBy + "%' ";

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
            List<CustomRedemptionGift> _data = new List<CustomRedemptionGift>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomRedemptionGift> _dataFiltered = new List<CustomRedemptionGift>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select id,name,type,points,frequency,");
                strFilteredQuery.Append("DATE_FORMAT(updated_at, '%Y-%m-%d %T') as updated_at, DATE_FORMAT(created_at, '%Y-%m-%d %T') as created_at from redeem ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomRedemptionGift>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region Get Redemption Gift Info
        /// <summary>
        /// Get Redemption Gift Info
        /// </summary>
        /// <returns></returns>
        public redeem GetRedemptionGiftInfo(long nID)
        {
            List<redeem> RedemptionGiftInfo;
            using (var db = new TittleEntities())
            {
                RedemptionGiftInfo = db.redeems.Where(x => x.id == nID).ToList();
            }
            return RedemptionGiftInfo[0];
        }
        #endregion

        #region Get Redemption Gift ByKey
        /// <summary>
        /// Get Redemption Gift ByKey
        /// </summary>
        /// <returns></returns>
        public redeem GetRedemptionGiftByKey(string name)
        {
            List<redeem> RedemptionGiftInfo;
            using (var db = new TittleEntities())
            {
                RedemptionGiftInfo = db.redeems.Where(x => x.name == name).ToList();
            }
            if (RedemptionGiftInfo.Count > 0)
                return RedemptionGiftInfo[0];
            else
                return null;
        }
        #endregion

        #region Delete Redemption Gift Info
        /// <summary>
        /// Delete Redemption Gift
        /// </summary>
        /// <returns></returns>
        public void DeleteRedemptionGift(long nID, ref string sMessage)
        {
            sMessage = "Delete can't be completed , there are ";
            redeem obj = GetRedemptionGiftInfo(nID);
            using (var db = new TittleEntities())
            {
                db.redeems.Attach(obj);
                db.redeems.Remove(obj);
                db.SaveChanges();
            }
            sMessage = "Success";
        }
        #endregion

        #region Save Or Update Redemption Gift
        /// <summary>
        /// Save or update Redemption Gift info
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdateRedemptionGift(CustomRedemptionGift _data, ref long nID)
        {
            string sMessage = "Success";

            redeem obj;
            if (_data.id != 0)
                obj = GetRedemptionGiftInfo(_data.id);
            else
                obj = new redeem();


            using (var db = new TittleEntities())
            {
                obj.type = _data.type;
                obj.name = _data.name;
                obj.points = _data.points;
                obj.frequency = _data.frequency;
                obj.updated_at = DateTime.Now;

                if (_data.id != 0)
                {
                    db.redeems.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                }
                else
                {
                    obj.created_at = DateTime.Now;
                    db.redeems.Add(obj);
                }
                db.SaveChanges();
                nID = obj.id;
            }

            return sMessage;
        }
        #endregion
    }
}
