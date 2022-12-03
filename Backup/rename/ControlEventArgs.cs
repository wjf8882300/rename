using System;
using System.Collections.Generic;
using System.Text;

namespace rename
{
	public class ControlEventArgs : EventArgs
	{
		public object param = null;
		public object Param
		{
			get {
				return param;
				}
				set
				{
					param = value;
				}
		}
		public ControlEventArgs(object param)
		{
			this.param = param;
		}
	}
}
