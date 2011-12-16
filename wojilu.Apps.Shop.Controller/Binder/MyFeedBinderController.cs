/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Domain;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;

namespace wojilu.Web.Controller.Shop.Binder {

    public class MyFeedBinderController : ControllerBase, ISectionBinder {

        public FeedService feedService { get; set; }
        public IShopCustomTemplateService ctService { get; set; }

        public MyFeedBinderController() {
            feedService = new FeedService();
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = getBlock( "list" );
            foreach (Feed feed in serviceData) {

                block.Set( "feed.UserFace", feed.Creator.PicSmall );
                block.Set( "feed.UserLink", Link.ToMember(feed.Creator) );


                String creatorInfo = string.Format( "<a href='{0}'>{1}</a>", Link.ToMember( feed.Creator ), feed.Creator.Name );
                String feedTitle = feedService.GetHtmlValue( feed.TitleTemplate, feed.TitleData, creatorInfo );
                block.Set( "feed.Title", feedTitle );
                block.Set( "feed.DataType", feed.DataType );

                String feedBody = feedService.GetHtmlValue( feed.BodyTemplate, feed.BodyData, creatorInfo );
                block.Set( "feed.Body", feedBody );
                block.Set( "feed.Created", cvt.ToTimeString(feed.Created) );

                block.Bind( "feed", feed );


                block.Next();
            }
        }


    }

}