using System;
using System.Windows;
using System.Windows.Controls;
using SharedPlaylist.Models;

namespace CommentSharedPlaylist.Utils
{
    public class CommentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OtherCommentDataTemplate { get; set; }
        public DataTemplate MyCommentDataTemplate { get; set; }

        public string Username { get; set; }
        

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itm = item as Comments;
            if (item != null)
            {

                if (itm.Username.Equals(Username,StringComparison.InvariantCultureIgnoreCase))
                {
                    return MyCommentDataTemplate;
                }

                return OtherCommentDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}