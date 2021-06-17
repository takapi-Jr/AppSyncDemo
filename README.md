# AppSyncDemo
Xamarin.Forms+AppSync(+Lambda)を実行するサンプル

## AppSyncスキーマ
```
type GetSampleData {
	message: String
	hoge: String
	fuga: String
	piyo: String
	foo: Int
	bar: Int
}

type GetSampleResponse {
	result: LambdaResult
	data: GetSampleData
}

type LambdaResult {
	status_code: Int
}

type Query {
	GetSample(message: String): GetSampleResponse
}

schema {
	query: Query
}
```

## AppSyncリクエストマッピングテンプレート
```
#**
The value of 'payload' after the template has been evaluated
will be passed as the event to AWS Lambda.
*#
{
  "version" : "2017-02-28",
  "operation": "Invoke",
  "payload": $util.toJson($context.args)
}
```

## AppSyncレスポンスマッピングテンプレート
```
$util.toJson($context.result)
```

## Lambda関数(GetSample)
```python
import json

def lambda_handler(event, context):
    # TODO implement
    print(event)
    
    result = {
        "status_code": 200
    }
    
    data = {
        "message": 'Hello {0}!!!'.format(event['message']),
        "hoge": "abc",
        "fuga": "あいう",
        "piyo": "xyz",
        "foo": 123,
        "bar": 456
    }

    ret = {
        "result": result,
        "data": data
    }
    
    return ret
```