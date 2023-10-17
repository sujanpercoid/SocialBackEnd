namespace TClone.Repository
{
    public interface IGenericRepository <T> where T : class
    {
       Task<string> Delete (object id) ;
        Task<string> Add (T entity) ;
        
    }
}
