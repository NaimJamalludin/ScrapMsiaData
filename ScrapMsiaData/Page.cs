using System.Collections.Generic;

namespace ScrapMsiaData
{
    /// <summary>
    /// Page Information.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Title of data.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL link to the title of data.
        /// </summary>
        public string UrlTitle { get; set; }

        /// <summary>
        /// Description of data.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Organization associated to data.
        /// </summary>
        public string Org { get; set; }

        /// <summary>
        /// URL link to the organization.
        /// </summary>
        public string UrlOrg { get; set; }

        /// <summary>
        /// Date creation
        /// </summary>
        public string DateCreated { get; set; }

        List<Resources> Resources = new List<Resources>();


    }
}
