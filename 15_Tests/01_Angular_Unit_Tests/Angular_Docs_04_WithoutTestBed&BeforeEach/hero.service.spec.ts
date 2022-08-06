import { MessageService } from './message.service';
import { HeroService } from './hero.service';
import { TestBed } from '@angular/core/testing';

describe('HeroService with Angular testing support', () => {
  function setup() {
    const messageServiceSpy = jasmine.createSpyObj('MessageService', ['add']);
    const heroService = new HeroService(messageServiceSpy);

    return { heroService, messageServiceSpy };
  }
  let heroService: HeroService;
  let messageServiceSpy: jasmine.SpyObj<MessageService>;

  it('#getHeroes should call add() once with string value', () => {
    //prepare
    const { heroService, messageServiceSpy } = setup();

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
    const { heroService, messageServiceSpy } = setup();
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
