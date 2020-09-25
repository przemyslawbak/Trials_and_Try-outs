import { MessageService } from './message.service';
import { HeroService } from './hero.service';

describe('HeroService without Angular testing support', () => {
  let heroService: HeroService;
  let messageServiceSpy = jasmine.createSpyObj(
    'MessageService',
    ['add'],
    ['clear']
  );
  heroService = new HeroService(messageServiceSpy);

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
