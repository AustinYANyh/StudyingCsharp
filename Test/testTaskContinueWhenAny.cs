//测试解除报警信息
                if (Constants._CurrentAlarmList.Count - toRemoveCount != 0)
                {
                    object lockobj = new object();
                    Constants._CurrentAlarmList.Last().IsRemoved = "已解除";
                    TaskFactory taskFactory = new TaskFactory();
                    List<Task> taskList = new List<Task>();
                    taskList.Add(taskFactory.StartNew(new Action(() =>
                    {
                        lock (lockobj)
                        {
                            toRemoveCount++;
                            BusinessLayerParamterService.Instance.AddToHistoryAlarmList(Constants._CurrentAlarmList.Last());
                            if (Constants._CurrentAlarmList.Count != 0)
                                Constants._CurrentAlarmList.Remove(Constants._CurrentAlarmList.Last());
                        }
                    })));
                    taskFactory.ContinueWhenAny(taskList.ToArray(), (t) =>
                     {
                         lock (lockobj)
                             toRemoveCount--;
                     });
                }
                DateTime current = DateTime.Now;
                Trace.WriteLine(current.Subtract(now).ToString());