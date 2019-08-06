using System.Text;

namespace Common.Helper
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 追加内容
        /// </summary>
        public static void Append( this string content, StringBuilder result, string value, params object[] args ) {
            if( string.IsNullOrWhiteSpace( value ) )
                return;
            result.Append( "   " );
            if( args == null || args.Length == 0 ) {
                result.Append( value );
                return;
            }
            result.AppendFormat( value, args );
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        public static void AppendLine( this string content, StringBuilder result, string value, params object[] args ) {
            content.Append( result, value, args );
            result.AppendLine();
        }

        /// <summary>
        /// 设置内容并换行
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="value">值</param>
        /// <param name="args">变量值</param>
        public static void Content( this string content, StringBuilder result,string value, params object[] args ) {
            content.AppendLine(result, value, args );
        }
    }
}
