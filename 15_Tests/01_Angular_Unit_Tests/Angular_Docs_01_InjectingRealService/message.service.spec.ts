import { MessageService } from './message.service';

describe('MessageService without Angular testing support', () => {
  let messageService: MessageService;

  it('#add should push fake message to fake messages array', () => {
    messageService = new MessageService();
    messageService.messages = ['one', 'two'];
    messageService.add('three');
    expect(messageService.messages.length).toBe(3);
    expect(messageService.messages[2]).toBe('three');
  });

  it('#add should clear messages array', () => {
    messageService = new MessageService();
    messageService.clear();
    expect(messageService.messages.length).toBe(0);
  });
});
