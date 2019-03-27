namespace project_management.DTO
{
    interface BaseDTO<T>
    {
        bool create(T obj);
        T read(int ID);
        bool update(T obj);
        List<T> list();
        T delete(int ID);

        private class NotImplementedException : Exception
        {
            public NotImplementedException(string msg)
            {
                super(msg);
            }
        }

    }
}
