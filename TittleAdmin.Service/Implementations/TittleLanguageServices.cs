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
    public class TittleLanguageServices
    {
        #region Translations list
        /// <summary>
        /// Translations list
        /// </summary>
        /// <returns></returns>
        public List<CustomLanguageTranslation> GetTranslationsList(string searchBy, int take, int skip, string sortBy, bool sortDir, string customField, out int filteredResultsCount, out int totalResultsCount)
        {
            var whereClause = "tbl.key like '%" + searchBy + "%' OR tbl.label like '%" + searchBy + "%' OR ";
            whereClause += "tbl.value like '%" + searchBy + "%' ";

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "key";
                sortDir = true;
            }

            if (sortBy.Contains("Date"))
            {
                sortBy = "STR_TO_DATE(" + sortBy + ", '%d/%m/%Y')";
            }

            if (sortDir) sortBy += " asc";
            else sortBy += " desc";
            List<CustomLanguageTranslation> _data = new List<CustomLanguageTranslation>();
            StringBuilder strQuery = new StringBuilder();
            List<CustomLanguageTranslation> _dataFiltered = new List<CustomLanguageTranslation>();
            StringBuilder strFilteredQuery = new StringBuilder();
            using (var db = new TittleEntities())
            {
                strFilteredQuery.Append("Select * from (");
                strFilteredQuery.Append("select k.key,k.label,t.value,t.id, t.key_id ");
                strFilteredQuery.Append("from `keys` as k join ");
                strFilteredQuery.Append("translations as t on k.id = t.key_id ");
                strFilteredQuery.Append("where t.language_id = " + customField + ") as tbl ");
                strFilteredQuery.Append("where " + whereClause);
                strFilteredQuery.Append(" order by tbl." + sortBy);
                _data = db.Database.SqlQuery<CustomLanguageTranslation>(strFilteredQuery.ToString()).ToList();
                _dataFiltered = _data.Skip(skip).Take(take).ToList();

                filteredResultsCount = _data.Count();
                totalResultsCount = _data.Count();
            }
            return _dataFiltered;
        }
        #endregion

        #region Languages list
        /// <summary>
        /// Languages list
        /// </summary>
        /// <returns></returns>
        public List<CustomLanguage> GetLanguagesList()
        {
            List<CustomLanguage> _dataQuery = new List<CustomLanguage>();
            using (var db = new TittleEntities())
            {
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT id as Id, label as LangLabel FROM languages ");
                _dataQuery = db.Database.SqlQuery<CustomLanguage>(strQuery.ToString()).ToList();
            }
            return _dataQuery;
        }
        #endregion

        #region Save Or Update Language
        /// <summary>
        /// Save or update Language info
        /// </summary>
        /// <param name="newLang"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdateLanguage(CustomNewLanguage newLang, ref long nID)
        {
            string sMessage = "Success";

            language obj = new language();


            using (var db = new TittleEntities())
            {
                obj.locale = newLang.locale;
                obj.label = newLang.langLabel;
                obj.created_at = DateTime.Now;
                obj.updated_at = DateTime.Now;
                db.languages.Add(obj);
                db.SaveChanges();

                //add entry in translation
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("INSERT INTO `translations` ");
                strQuery.Append("(`language_id`,`key_id`,`value`,`created_at`,`updated_at`) ");
                strQuery.Append("select "+obj.id.ToString() + ",id,'',NOW(),NOW() from `keys`");
                var _dataQuery = db.Database.ExecuteSqlCommand(strQuery.ToString());
                nID = obj.id;
            }

            return sMessage;
        }
        #endregion
        #region Delete Language
        /// <summary>
        /// Delete Language info
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string DeleteLanguage(long nID)
        {
            string sMessage = "Success";

            language obj = new language();


            using (var db = new TittleEntities())
            {
                obj = db.languages.Where(x => x.id == nID).FirstOrDefault();
                if (obj != null && obj.id > 0)
                {
                    //delete translation data
                    StringBuilder strQuery = new StringBuilder();
                    strQuery.Append("Delete from `translations` ");
                    strQuery.Append("WHERE language_id=" + nID.ToString() + " AND id!=0");
                    var _dataQuery = db.Database.ExecuteSqlCommand(strQuery.ToString());

                    //delete language
                    db.languages.Attach(obj);
                    db.languages.Remove(obj);
                    db.SaveChanges();

                    nID = obj.id;
                }
                else
                {
                    sMessage = "Language not exists.";
                }
            }

            return sMessage;
        }
        #endregion

        #region Save Or Update Key
        /// <summary>
        /// Save or update Key info
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string SaveOrUpdateKey(key newKey, ref long nID)
        {
            string sMessage = "Success";

            key obj = new key();


            using (var db = new TittleEntities())
            {
                obj = db.keys.Where(x => x.key1 == newKey.key1).FirstOrDefault();
                if (obj == null || obj.id == 0)
                {
                    newKey.created_at = DateTime.Now;
                    newKey.updated_at = DateTime.Now;
                    db.keys.Add(newKey);
                    db.SaveChanges();

                    //add entry in translation
                    StringBuilder strQuery = new StringBuilder();
                    strQuery.Append("INSERT INTO `translations` ");
                    strQuery.Append("(`language_id`,`key_id`,`value`,`created_at`,`updated_at`) ");
                    strQuery.Append("select id," + newKey.id.ToString() + ",'',NOW(),NOW() from languages");
                    var _dataQuery = db.Database.ExecuteSqlCommand(strQuery.ToString());
                    nID = newKey.id;
                }
                else
                {
                    sMessage = "Key already exist.";
                }
            }

            return sMessage;
        }
        #endregion
        #region Delete key
        /// <summary>
        /// Delete key info
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public string Deletekey(long nID)
        {
            string sMessage = "Success";

            key obj = new key();


            using (var db = new TittleEntities())
            {
                obj = db.keys.Where(x => x.id == nID).FirstOrDefault();
                if (obj != null && obj.id > 0)
                {
                    //delete translation data
                    StringBuilder strQuery = new StringBuilder();
                    strQuery.Append("Delete from `translations` ");
                    strQuery.Append("WHERE key_id=" + nID.ToString() + " AND id!=0");
                    var _dataQuery = db.Database.SqlQuery<CustomLanguage>(strQuery.ToString()).ToList();

                    //delete key
                    db.keys.Attach(obj);
                    db.keys.Remove(obj);
                    db.SaveChanges();

                    nID = obj.id;
                }
                else
                {
                    sMessage = "Key not exists.";
                }
            }

            return sMessage;
        }
        #endregion
        #region Update key
        /// <summary>
        /// Update key info
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Updatekey(long nID, string value)
        {
            string sMessage = "Success";

            key obj = new key();


            using (var db = new TittleEntities())
            {
                obj = db.keys.Where(x => x.id == nID).FirstOrDefault();
                if (obj != null && obj.id > 0)
                {
                    obj.label = value;
                    obj.updated_at = DateTime.Now;
                    //update key
                    db.keys.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    nID = obj.id;
                }
                else
                {
                    sMessage = "Key not exists.";
                }
            }

            return sMessage;
        }
        #endregion
        #region Update translation
        /// <summary>
        /// Update translation info
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string UpdateTranslation(long nID, string value)
        {
            string sMessage = "Success";

            translation obj = new translation();


            using (var db = new TittleEntities())
            {
                obj = db.translations.Where(x => x.id == nID).FirstOrDefault();
                if (obj != null && obj.id > 0)
                {
                    obj.value = value;
                    obj.updated_at = DateTime.Now;
                    //update key
                    db.translations.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    nID = obj.id;
                }
                else
                {
                    sMessage = "Translation not exists.";
                }
            }

            return sMessage;
        }
        #endregion
    }
}
