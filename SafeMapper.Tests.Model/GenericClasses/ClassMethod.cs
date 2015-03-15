namespace SafeMapper.Tests.Model.GenericClasses
{
    public class ClassMethod<T>
    {
        private T value;

        public void SetValue(T val)
        {
            this.value = val;
        }

        public T GetValue()
        {
            return this.value;
        }
    }
}
