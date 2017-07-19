using PersonalBlog.Logic;
using PersonalBlog.LogicContracts;

namespace PersonalBlog.WebUI
{
    public static class LogicProvider
    {
        static LogicProvider()
        {
            ArticleLogic = new ArticleLogic();
            TagLogic = new TagLogic();
            UserLogic = new UserLogic();
            RoleLogic = new RoleLogic();
            CommentLogic = new CommentLogic();
        }

        public static IArticleLogic ArticleLogic { get; }

        public static ITagLogic TagLogic { get; }

        public static IUserLogic UserLogic { get; }

        public static IRoleLogic RoleLogic { get; }

        public static ICommentLogic CommentLogic { get; }
    }
}