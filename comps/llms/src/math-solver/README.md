# LLMS Math Solver microservice

To build the Docker image

``` BASH
docker build -t ${REGISTRY}/mathsolver-tgi:latest .
```

Run the service using docker compose

``` BASH
docker compose -f docker_compose.yaml up -d
```

## API definition

### Request

To consume the service follow the OpenAI chat interface

``` BASH
$ curl --location '${host}:${port}/v1/chat/completions' \
--header 'Content-Type: application/json' \
--data '{
    "model": "meta-llama/Llama-3.1-8B-Instruct",
    "messages": [
      {
        "role": "student",
        "content": "2x+3=11"
      }
    ]
}'
```

### Response

The response of the service is

``` JSON
{
    "id": "",
    "object": "chat.completion",
    "created": 1740546448,
    "model": "meta-llama/Llama-3.1-8B-Instruct",
    "choices": [
        {
            "index": 0,
            "message": {
                "role": "system",
                "content": "{\n    \"answer\" : \"4\",\n    \"steps\" : [\n        {\n            \"step\" : 1,\n            \"detail\" : \"Substract 3 to both sides of the equation\"\n        },\n        {\n            \"step\" : 2,\n            \"detail\" : \"Now you have 2x = 8\"\n        },\n        {\n            \"step\" : 3,\n            \"detail\" : \"Divide by 2 both sides of the equation\"\n        },\n        {\n            \"step\" : 4,\n            \"detail\" : \"Now you have x = 4\"\n        }\n    ]\n}"
            },
            "finishReason": "stop",
            "metadata": ""
        }
    ],
    "usage": {
        "promptTokens": 375,
        "totalTokens": 506,
        "completionTokens": 131
    }
}
```
