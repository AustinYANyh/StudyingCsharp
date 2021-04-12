/*============================================================================================
   *   Copyright (C) 2021 All rights reserved.
   *   
   *   文件名称：Singleton.cs
   *   创 建 者：yanyunhao
   *   创建日期：2021/4/12 15:45:42
   *   描    述：
==============================================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace System {
    public static class Singleton<TItem> where TItem : class, new() {
		public static TItem GetInstance() {
			if (_Instance == null) {
				Interlocked.CompareExchange<TItem>(ref _Instance, Activator.CreateInstance<TItem>(), default(TItem));
			}
			return _Instance;
		}
		private static TItem _Instance = default(TItem);
	}
}
