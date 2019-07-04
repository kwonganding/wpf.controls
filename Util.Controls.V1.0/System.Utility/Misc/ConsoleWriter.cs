using System;
using System.IO;
using System.Text;

namespace System.Utility
{
    /// <summary>
    /// 自定义Console输出
    /// </summary>
    public class ConsoleWriter : TextWriter
    {
        /// <summary>
        /// 当有Console输出时触发的操作
        /// </summary>
        public Action<string> OnWirte { get; set; }

        #region override TextWriter

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            this.DoWrite(value);
        }

        public override void Write(bool value)
        {
            this.DoWrite(value ? "true" : "false");
        }

        public override void Write(int value)
        {
            this.DoWrite(value);
        }

        public override void Write(uint value)
        {
            this.DoWrite(value);
        }

        public override void Write(long value)
        {
            this.DoWrite(value);
        }

        public override void Write(ulong value)
        {
            this.DoWrite(value);
        }

        public override void Write(float value)
        {
            this.DoWrite(value);
        }

        public override void Write(double value)
        {
            this.DoWrite(value);
        }

        public override void Write(decimal value)
        {
            this.DoWrite(value);
        }

        public override void Write(string value)
        {
            this.DoWrite(value);
        }

        public override void Write(object value)
        {
            this.DoWrite(value);
        }

        public override void Write(string format, params object[] arg)
        {
            this.DoWrite(string.Format(format, arg));
        }

        public override void WriteLine()
        {
            this.DoWriteLine(string.Empty);
        }

        public override void WriteLine(char value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(bool value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(int value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(long value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(float value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(double value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(decimal value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(string value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(object value)
        {
            this.DoWriteLine(value);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.DoWriteLine(string.Format(format, arg));
        }
        #endregion

        /****************** private methods ******************/

        private void DoWrite(object value)
        {
            if (this.OnWirte != null)
            {
                this.OnWirte(value.ToSafeString());
            }
        }

        private void DoWrite(string value)
        {
            if (this.OnWirte != null)
            {
                this.OnWirte(value);
            }
        }

        private void DoWriteLine(object value)
        {
            if (this.OnWirte != null)
            {
                this.OnWirte(string.Concat(Environment.NewLine, value.ToSafeString()));
            }
        }

        private void DoWriteLine(string value)
        {
            if (this.OnWirte != null)
            {
                this.OnWirte(string.Concat(Environment.NewLine, value));
            }
        }
    }
}