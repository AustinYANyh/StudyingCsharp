using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORAPS.Helpers;

namespace ORAPS.Fabric.Data
{
    [ProtoBuf.ProtoContract(InferTagFromName = true, ImplicitFields = ProtoBuf.ImplicitFields.AllPublic)]
    public class Student : DataObject
    {
        public Student()
        {
            ID = -1;
            Name = ORAPS.Helpers.HelperStrs._Empty;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        #region DataObject
        private object _OriginalValue;
        [ProtoBuf.ProtoIgnore()]
        public object OriginalValue
        {
            get { return _OriginalValue; }
            set { _OriginalValue = value; }
        }

        public string OrgValStr { get; set; }
        private DbStatus _State;
        public DbStatus State
        {
            get { return _State; }
            set { _State = value; }
        }

        public bool DataSelected { get; set; }
        public string ErrorMessage { get; set; }
        public bool CheckUpdate(QuickReflector reflector, List<ORAPS.Helpers.FieldInfo> Infos)
        {
            return DataHelper.CheckUpdate(reflector, Infos, this);
        }
        public static bool checkDBEqual(object tgtObj, List<ORAPS.Helpers.FieldInfo> KeyInfos, List<Func<object, object>> GetDels)
        {
            return DataHelper.CheckDBEqual(tgtObj, KeyInfos, GetDels);
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
