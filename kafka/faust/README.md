# Faust, Kafka Stream in Pyhon

[faust source repo](https://github.com/robinhood/faust)

In its own words: Faust is a stream processing library, porting the ideas from Kafka Streams to Python.

This is really a smart Kafka Streams solution that is simple and easy for developers to implement!

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

### Using periodic task to generate Kafka activities
```python
@app.timer(interval=10.0)
async def example_sender():
    """
        generate some data with a periodic job
    """
    await hello.send(
        value=Greeting(from_name='Faust', to_name='you', sent_at=datetime.datetime.now()),
    )

```

### Generate activities using faust command
```python
@app.command()
async def send_value():
    """
        use command line to send => faust -A helloworld send_value
    """
    await topic.send(
        value=Greeting(from_name='Bruno', to_name='you', sent_at=datetime.datetime.now()),
    )
```
