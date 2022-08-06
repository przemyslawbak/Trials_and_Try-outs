import { MessageService } from './message.service';
import { HeroService } from './hero.service';
import { TestBed } from '@angular/core/testing';

describe('HeroService with Angular testing support', () => {
  let heroService: HeroService;
  let messageServiceSpy: jasmine.SpyObj<MessageService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('MessageService', ['add']);

    TestBed.configureTestingModule({
      // Provide both the service-to-test and its (spy) dependency
      providers: [HeroService, { provide: MessageService, useValue: spy }],
    });
    // Inject both the service-to-test and its (spy) dependency
    heroService = TestBed.inject(HeroService);
    messageServiceSpy = TestBed.inject(MessageService) as jasmine.SpyObj<
      MessageService
    >;
  });

  it('#getHeroes should call add() once with string value', () => {
    //prepare
    messageServiceSpy.add.calls.reset();

    //act
    heroService.getHeroes();

    //assert
    expect(messageServiceSpy.add).toHaveBeenCalledTimes(1);
    expect(messageServiceSpy.add).toHaveBeenCalledWith(
      'HeroService: fetched heroes'
    );
  });

  it('#getHero should call add() once with proper string value', () => {
    //prepare
    messageServiceSpy.add.calls.reset();
    let id = 1;

    //act
    heroService.getHero(id);

    //assert
    expect(messageServiceSpy.add).toHaveBeenCalledTimes(1);
    expect(messageServiceSpy.add).toHaveBeenCalledWith(
      'HeroService: fetched hero id=' + id
    );
  });
});
