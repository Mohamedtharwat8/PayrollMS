using System.Collections;
 
namespace PayrollMS.Helpers
{
    public class ResponseResult<T>
    {
        private int? _TotalCount = 0;
        public string Status { get; set; }
        public string Msg { get; set; }
        public T Result { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime ServerDateTime { get; } = DateTime.Now;
        public int ResponseId { get; set; } = 0;

        public int? CurrentCount
        {
            get
            {
                if (Result == null)
                {
                    _TotalCount = 0;
                    return 0;

                }
                else if (Result.IsList())
                {
                    IList enumerable = (IList)Result;
                    var count = enumerable.Count;
                    if (count > 0)
                    {
                        var item = enumerable[0].GetType().GetProperty("MaxRows");
                        if (item != null)
                        {
                            var total = item.GetValue(enumerable[0], null);
                            if (total != null)
                                _TotalCount = total.ToInt();
                            else
                                _TotalCount = count;
                        }
                        else
                        {
                            _TotalCount = count;
                        }
                    }
                    return count;
                }
                else if (Result.IsDictionary())
                {
                    IDictionary dictionary = (IDictionary)Result;
                    _TotalCount = dictionary.Keys.Count;
                    return _TotalCount;
                }
                else
                {
                    _TotalCount = 1;
                    return 1;
                }
            }
        }
        public int? TotalCount { get => _TotalCount; }

        public ResponseResult()
        {

        }

        public ResponseResult(T Result)
        {
            this.Result = Result;
        }
        public ResponseResult(string Status, string Msg, T Result, string AdditionalInformation = "")
        {
            this.Status = Status;
            this.Msg = Msg;
            this.Result = Result;
            this.AdditionalInformation = AdditionalInformation;
        }

        public ResponseResult(string Status, string Msg, T Result, int responseId, string AdditionalInformation = "")
        {
            this.Status = Status;
            this.Msg = Msg;
            this.Result = Result;
            this.ResponseId = responseId;
            this.AdditionalInformation = AdditionalInformation;
        }

    }
}
