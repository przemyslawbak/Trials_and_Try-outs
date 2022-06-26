using System;

namespace GettingStarted.Models
{
	/*
	 * It is strongly suggested to use interfaces for message contracts,
	 * based on experience over several years with varying levels of developer experience.
	 * MassTransit will create dynamic interface implementations for the messages, ensuring
	 * a clean separation of the message contract from the consumer.
	*/

    public interface UpdateCustomerAddress
	{
		Guid CommandId { get; }
		DateTime Timestamp { get; }
		string CustomerId { get; }
		string HouseNumber { get; }
		string Street { get; }
		string City { get; }
		string State { get; }
		string PostalCode { get; }
	}
}
