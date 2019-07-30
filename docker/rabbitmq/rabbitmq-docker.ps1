$params = @(
    '--hostname', "rabbit-test-server",
    '--name', 'rabbit-test-server',
    '-e', 'RABBITMQ_DEFAULT_USER=bunny',
     '-e', 'RABBITMQ_DEFAULT_PASS=bugs',
     '-p', '8080:15672',
     '-p', '5672:5672',
    # '-P',
    '-d', 
    'rabbitmq:3.7.16-management-alpine'
)
docker run @params
# docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
