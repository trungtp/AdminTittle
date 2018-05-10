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
    public class TittlePromoCodeServices
    {
        #region Promo Code list
        /// <summary>
        /// Promo Codes list
        /// </summary>
        /// <returns></returns>
        public List<CustomPromoCode> GetPromoCodesList(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "CodeID like '%" + searchBy + "%' OR Quantity like '%" + searchBy + "%' OR ";
            whereClause += "Value like '%" + searchBy + "%' OR Rules like '%" + searchBy + "%' OR ";
            whereClause += "StartDate like '%" + searchBy + "%' OR EndDate like '%" + searchBy + "%' OR ";
            whereClause += "Description like '%" + searchBy + "%' OR TypeValue like '%" + searchBy + "%' OR ";
            whereClause += "Status like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "CodeID";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%d/%m/%Y')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomPromoCode> _data = new List<CustomPromoCode>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomPromoCode> _dataFiltered = new List<CustomPromoCode>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("SELECT id, code as CodeID, description as Description, promo_type as TypeValue, ");
                strFilteredQuery.Append("if (type = 'percentage',concat(FLOOR(value), '%'),concat(value, ' SGD')) as Value,  ");
                strFilteredQuery.Append("DATE_FORMAT(start_date, '%d/%m/%Y') as StartDate,  ");
                strFilteredQuery.Append("DATE_FORMAT(end_date, '%d/%m/%Y') as EndDate, ");
                strFilteredQuery.Append("rule as Rules, if(quantity=-1,'Unlimited',quantity) as Quantity, ");
                strFilteredQuery.Append("if (now() < start_date,'Scheduled',if (now() >= end_date,'Expired','Open')) as Status ");
                strFilteredQuery.Append("FROM promo_codes) as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by " + sortBy);
                _data = db.Database.SqlQuery<CustomPromoCode>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region Get Promo Code Info
        /// <summary>
        /// Get Promo Code Info
        /// </summary>
        /// <returns></returns>
        public promo_codes GetPromoCodeInfo(long nID)
        {
            List<promo_codes> PromoCodeInfo;
            using (var db = new TittleEntities())
            {
                PromoCodeInfo = db.promo_codes.Where(x => x.id == nID).ToList();
            }
            return PromoCodeInfo[0];
        }
        #endregion

        #region Get Promo Code Info By Code
        /// <summary>
        /// Get Promo Code Info By Code
        /// </summary>
        /// <returns></returns>
        public promo_codes GetPromoCodeInfoByCode(string code)
        {
            List<promo_codes> PromoCodeInfo;
            using (var db = new TittleEntities())
            {
                PromoCodeInfo = db.promo_codes.Where(x => x.code == code).ToList();
            }
            if (PromoCodeInfo.Count > 0)
                return PromoCodeInfo[0];
            else
                return null;
        }
        #endregion

        #region Delete Promo Code Info
        /// <summary>
        /// Delete Promo Code
        /// </summary>
        /// <returns></returns>
        public void DeletePromoCode(long nID, ref string sMessage)
        {
            sMessage = "Delete can't be completed , there are ";
            promo_codes obj = GetPromoCodeInfo(nID);
            using (var db = new TittleEntities())
            {
                db.promo_codes.Attach(obj);
                db.promo_codes.Remove(obj);
                db.SaveChanges();
            }
            sMessage = "Success";
        }
        #endregion

        #region Save Or Update Promo Code
        /// <summary>
        /// Save or update promo code info
        /// </summary>
        /// <param name="_promoCode"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdatePromoCode(CustomPromoCode _promoCode, ref long nID)
        {
            string sMessage = "Success";

            promo_codes obj;
            if (_promoCode.id != 0)
                obj = GetPromoCodeInfo(_promoCode.id);
            else
                obj = new promo_codes();


            using (var db = new TittleEntities())
            {
                obj.code = _promoCode.CodeID;
                obj.description = _promoCode.Description;
                obj.end_date = DateTime.ParseExact(_promoCode.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj.promo_type = _promoCode.TypeValue;
                obj.quantity = Convert.ToInt32(_promoCode.Quantity);
                obj.rule = string.IsNullOrEmpty(_promoCode.Rules) ? "" : _promoCode.Rules;
                obj.start_date = DateTime.ParseExact(_promoCode.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj.type = _promoCode.type;
                obj.value = Convert.ToDecimal(_promoCode.Value);
                obj.updated_at = DateTime.Now;

                if (_promoCode.id != 0)
                {
                    db.promo_codes.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                }
                else
                {
                    obj.created_at = DateTime.Now;
                    db.promo_codes.Add(obj);
                }
                db.SaveChanges();
                nID = obj.id;
            }
            
            return sMessage;
        }
        #endregion
    }
}
