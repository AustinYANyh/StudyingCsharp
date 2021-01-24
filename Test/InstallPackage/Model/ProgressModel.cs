using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installationpackage.Model
{
	public class ProgressModel : NotificationObject
	{
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
				this.RaisePropertyChanged("Level");
			}
		}

		public int Progress
		{
			get
			{
				return this._Progress;
			}
			set
			{
				this._Progress = value;
				this.RaisePropertyChanged("Progress");
			}
		}

		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
				this.RaisePropertyChanged("Message");
			}
		}

		private int _Level = 1;

		private int _Progress;

		private string _Message = "正在准备安装程序...";
	}
}
