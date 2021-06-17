using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AppSyncDemo.Models
{
    [DataContract]
    public class SampleModel
    {
        // Lambdaから返却される値のうち、GraphQLで必要な要素のみを選択できる

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "hoge")]
        public string Hoge { get; set; }

        [DataMember(Name = "fuga")]
        public string Fuga { get; set; }

        [DataMember(Name = "foo")]
        public int? Foo { get; set; }

#if false
// AWSコンソールで実行時のレスポンス例
{
  "data": {
    "GetSample": {
      "result": {
        "status_code": 200
      },
      "data": {
        "message": "Hello Xamarin!!!",
        "hoge": "abc",
        "fuga": "あいう",
        "piyo": "xyz",
        "foo": 123,
        "bar": 456
      }
    }
  }
}
#endif
    }
}
