using System;

namespace Alpalis.UtilityServices.Models
{
    [Serializable]
    public class EnabledUI
    {
        #region Class Constructor
        public EnabledUI()
        {
            ID = new ushort();
            Key = new short();
            Visible = new bool();
            IsMain = new bool();
        }
        #endregion Class Constructor

        public ushort ID { get; set; }

        public short Key { get; set; }

        public bool Visible { get; set; }

        public bool IsMain { get; set; }
    }
}
