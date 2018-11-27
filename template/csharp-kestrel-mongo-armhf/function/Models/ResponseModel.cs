namespace Function
{
    /// <summary>
    /// A Response model.
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// An object that represents the http response.
        /// </summary>
        public object response { get; set; }

        /// <summary>
        /// An integer that represents the http response status code.
        /// </summary>
        public int status { get; set; }
    }
}