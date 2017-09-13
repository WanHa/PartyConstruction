namespace DTcms.BLL
{
    public class Feedback
    {
        private readonly DAL.P_AuditingFeedback dal;
        public Feedback()
        {
            dal = new DAL.P_AuditingFeedback();
        }
        public string Add(Model.P_AuditingFeedback model, string id)
        {
            return dal.Add(model, id);
        }
    }
}