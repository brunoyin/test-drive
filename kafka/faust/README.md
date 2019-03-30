# Faust, Kafka Stream in Pyhon

[faust source repo](https://github.com/robinhood/faust)

In its own words: Faust is a stream processing library, porting the ideas from Kafka Streams to Python.

This is really a smart, simple and easy Kafka stream solution!

## Testing

- Start a Kafka Docker container using images from https://github.com/wurstmeister/kafka-docker
- Create a simple [helloworld](./helloworld.py) faust app
- Start the faust app worker
- Generate some activities

### Docker-compose to start both Zookeeper and Kalfka

[docker-compose.yml](..\..\docker\kafka\docker-compose.yml)

Run:
```bash
docker-compose up -d
```

### Running helloworld worker

```bash
faust -A helloworld worker -l info
```

### Generate activities using faust command
```python
@app.command()
async def send_value():
    """
        use command line to send
    """
    await topic.send(
        value=Greeting(from_name='Bruno', to_name='you', sent_at=datetime.datetime.now()),
    )
```
