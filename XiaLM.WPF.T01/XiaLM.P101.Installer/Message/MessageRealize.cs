using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Log;
using XiaLM.Utility;

namespace XiaLM.P101.Installer.Message
{
    public class MessageRealize
    {
        [ImportMany(typeof(IMessageEvent))]
        public IEnumerable<IMessageEvent> messages { get; set; }
        private static readonly object lockObj = new object();
        private static MessageRealize initialize;
        public static MessageRealize GetInstance()
        {
            if (initialize == null)
            {
                lock (lockObj)
                {
                    if (initialize == null)
                    {
                        initialize = new MessageRealize();
                    }
                }
            }
            return initialize;
        }
        private MessageRealize()
        {
            ModuleComposePart.GetInstance().ComposePart(this);
        }
        /// <summary>
        /// 发送事件消息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task SendMsg(EventArgs args)
        {
            try
            {
                foreach (var item in messages)
                {
                    if (item.EventArgsType.Equals(args.GetType()))
                    {
                        Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                await item.OnMessage(args);
                            }
                            catch (Exception ex)
                            {
                            }
                        }).Employ();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            await Task.CompletedTask;
        }
    }
}
