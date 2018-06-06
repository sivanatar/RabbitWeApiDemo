# RabbitWeApiDemo

# Pre-Requisite:
	RabbitMQ, Erlang
	DotNetCore 2.0
	RabbitMQ.Client

# Supported WebAPI Methods:
	To post/add Author resource
		http://localhost:62920/api/Author/Add

			Author data to post:
				{"Id":1, "FirstName":"Steve", "LastName":"McConnell"}

	To post/add Book resource
		http://localhost:62920/api/Book/Add

			Book data to post:
				{"Id":1001, "Title":"Code Complete: A Practical Handbook of Software Construction"}

	To get the first added Author
		http://localhost:62920/api/Author/Get

	To get the first added Book
		http://localhost:62920/api/Book/Get

	To get all the Authors
		http://localhost:62920/api/Author/All

	To get the first added Books
		http://localhost:62920/api/Book/Get

# Future Enhancements:
	Exception handling like handling the RabbitMQ related failures.
	Methods(Get/All/Post) with Controllers can be abstracted.
	To take certain values like queue host and queue name can be taken from configuration file.
