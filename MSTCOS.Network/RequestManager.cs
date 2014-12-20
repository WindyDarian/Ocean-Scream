using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MSTCOS.Network
{
    public class RequestManager : GameComponent 
    {

        List<AIRequest> requestList = new List<AIRequest>(1000000);

        public RequestManager(Game mainGame)
            : base(mainGame)
        {

        }

        public void addRequest(AIRequest request)
        {
            requestList.Add(request);
        }

        public override void Update(GameTime gameTime)
        {
            if (requestList.Count > 0)
            {
                //requestSort();
                for (int i = 0; i < requestList.Count; i++)
                {
                    requestList[i].deal();
                }
                requestList.Clear();
            }
            base.Update(gameTime);
        }

        //public void requestSort()
        //{
        //    AIRequest temp;
        //    int l = 0;
        //    for (int i = 0; i < requestList.Count; i++)
        //        if (requestList[i].type == "Attack")
        //        {
        //            temp = requestList[i];
        //            requestList[i] = requestList[l];
        //            requestList[l] = temp;
        //            l++;
        //        }
        //}

        public void clearAll()
        {
            requestList.Clear();
        }

    }
}
