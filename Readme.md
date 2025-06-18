This is my solution for the *Glass Lewis* take home assignment, as part of my *Capaciteam* interview.

# Solution Components

- Web API written in **C#** using **.NET 9**
- Frontend client implemented using **Angular 20**  
- **PostgreSQL** database for persistent data store
- **Authentication** using JWT tokens and **Duende Identity Server**
- Containerized application that can be ran with one command without prior configuration
- Containerized testing suite

# Running the application

### Setting everything up

Clone the repository with:

`git clone https://github.com/DaniloDjokic/glass-lewis-test.git`

Cd into the folder.

`cd glass-lewis-test`

Run the application with Docker Compose using:

`docker compose up --build`

This command will pull and build the necessary images to run the entire application. Make sure **Docker is installed** on the host machine. If it is not, the Docker engine can be downloaded for you target architecture at https://docs.docker.com/engine/install

Docker compose may take some time on initial build since it will pull the PostgreSQL image form the registry and build the 3 local images.

Once Docker compose completes the startup process the services will be available locally at the following addresses:

- API `http://localhost:8080`
- Angular Client `http://localhost:8081`
- PostgreSQL Database `http://localhost:8082`
- IdentityServer `http://localhost:8083`

### Exploring the API

The Web API comes built in with an API explorer that can be used to test the various API endpoints. 
It is available at `http://localhost:8080/scalar/v1` and looks like this:

###Screenshot of Scalar

Since the API is protected by JWT authentication you will need a bearer token to properly explore the endpoints. You can get one using the following command:

`curl -X POST http://localhost:8083/connect/token  -H "Content-Type: application/x-www-form-urlencoded" -d "client_id=glass-lewis-api&client_secret=very-secure-development-secret&grant_type=password&username=admin&password=admin&scope=api"`

### Using the Angular client

Upon visiting `http://localhost:8081` you will be presented with a login screen:

###Screenshot of login

The authentication configuration is seeded with a test user that you can use to login:

- Username: admin
- Password: admin

Once logged in, you will be presented with a company listing screen

###Scrrenshot of company list

Here you can interact with the companies in the database by:

- Searching them by Id
- Searching them by ISIN
- Searching them by all fields
- Adding a new company
- Updating an existing company

# Running tests

The tests in this application can be ran in 2 ways:

##### Locally

This gives a nicer test output but requires the .NET 9 SDK to be installed. In order to run tests like this you need to cd into the api directory with `cd api`

and the run `dotnet test`

##### In Docker

The tests can also be ran inside a docker container. Cd into the same directory (`cd api`).
Then, you will need to build the testing image, run:

`docker build -f Dockerfile.test -t glass-lewis-api-tests .`

And then run the image using

`docker run glass-lewis-api-tests`

### Bonus: Generating code coverage

Generating code coverage entails some additional configuration.

First, install the dotnet coverage tool:
`dotnet tool install --global dotnet-coverage`

Then, install the report generation tool:
`dotnet tool install --global dotnet-reportgenerator-globaltool`

Run the following command in order to collect code coverage information about the tests

`dotnet-coverage collect "dotnet test" --output coverage.cobertura.xml --output-format cobertura`

Then, run the command to convert the xml output into a human readable format with

`reportgenerator -reports:coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html`

(Note, if you get an error that 'reportgenerator' is not a recognized dommand, try 'dotnet reportgenerator')

Once the command successfully runs, you can open the generated code coverage file. It is locates in `coveragereport/index.htm` and looks something like this:

###Screenshot code coverage

# Production considerations

This solution, while robust, is very much **not** production ready. Some things that should first be cleared up include:

- **Configuring the API for https**: I purposefully avoided this here in order to preserve the "plug-and-play" nature of Docker Compose and avoid the complications of self-signed certificates
- **Protection of signing keys**: Currently, the identity server configuration uses developer signing credentials to sign JWT tokens. For production we could use something like *Azure Key Vault* to safely store these signing keys
- **Load testing**: The API has not been load tested. We could use something like *Bogus* to load the database with a bunch of dummy data and load test the endpoints with *K6*
- **CI/CD** For production we would need a proper CI/CD pipeline that can run tests and make it easier to make changes
- API Rate limiting and throttling
- Enable audit logging for authentication events
- Setting up automated database backups
- Implementing caching strategies for heavy read operations that are frequently executed using something like *Redis*
