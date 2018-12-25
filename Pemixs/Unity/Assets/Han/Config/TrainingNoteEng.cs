using UnityEngine;
namespace Remix{
	public class TrainingNoteEng{
		public const int ID_COUNT = 43;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TrainingNoteEng Get(string key){
			switch (key) {
			
			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TrainingNoteEng Get(int key){
			switch (key) {
			case 0: return new TrainingNoteEng{Desc="Welcome to Meow Remix! Here you will learn about how to master the game. "};
case 1: return new TrainingNoteEng{Desc="Follow the prompt and hit the button in matching color. See the kitty dance to the beat."};
case 2: return new TrainingNoteEng{Desc="Press the correct paw pad when the prompt marker reaches the “hit zone” on the far left. "};
case 3: return new TrainingNoteEng{Desc="Now brace yourself for some real challenge! "};
case 4: return new TrainingNoteEng{Desc="Press the correct paw pad when the prompt marker reaches the “hit zone” on the far left. "};
case 5: return new TrainingNoteEng{Desc="Great! Next is Continuously hitting the paw."};
case 6: return new TrainingNoteEng{Desc="Continuously hit the paw when the prompt marker reaches the “hit zone” on the far left. "};
case 7: return new TrainingNoteEng{Desc="Good! Next is up arrow of red marker."};
case 8: return new TrainingNoteEng{Desc="SLIDE upward on the central paw pad when the up arrow of red marker reaches the “hit zone” on the far left. "};
case 9: return new TrainingNoteEng{Desc="Nice! Down arrow of red marker."};
case 10: return new TrainingNoteEng{Desc="SLIDE downward on the central paw pad when the down arrow of red marker reaches the “hit zone” on the far left. "};
case 11: return new TrainingNoteEng{Desc="Awesome! Next is Continuously twisting the paw."};
case 12: return new TrainingNoteEng{Desc="Continuously twist the paw when the prompt marker reaches the “hit zone” on the far left. "};
case 13: return new TrainingNoteEng{Desc="Fantastic! You’ve learned everything! "};
case 14: return new TrainingNoteEng{Desc="Let's get started on the Interactive mode. Cats have different moods."};
case 15: return new TrainingNoteEng{Desc="Click on cat's profile picture. It will open the interactive window."};
case 16: return new TrainingNoteEng{Desc="When the cat is happy，pet the cat will add his EXP."};
case 17: return new TrainingNoteEng{Desc="Too much petting will set him off."};
case 18: return new TrainingNoteEng{Desc="When the cat is angry， use catnip to cool him down."};
case 19: return new TrainingNoteEng{Desc="When the cat is excited，he will want to play."};
case 20: return new TrainingNoteEng{Desc="Use a toy to play with the cat%2c will add his EXP."};
case 21: return new TrainingNoteEng{Desc="Different body type of cats likes different kinds of toys."};
case 22: return new TrainingNoteEng{Desc="Cat will be hungry after losing HP in levels."};
case 23: return new TrainingNoteEng{Desc="Feed the cat to add HP."};
case 24: return new TrainingNoteEng{Desc="Cat will fall asleep after playing 5 levels."};
case 25: return new TrainingNoteEng{Desc="Pet the cat to wake him up."};
case 26: return new TrainingNoteEng{Desc="Although the cat will be angry%2c but he still can play levels."};
case 27: return new TrainingNoteEng{Desc="Don't forget to play with your cats everyday."};
case 28: return new TrainingNoteEng{Desc="Let's get started on the Eexplore mode. You will find different cats in different maps."};
case 29: return new TrainingNoteEng{Desc="Click on the button to switch into Explore mode. "};
case 30: return new TrainingNoteEng{Desc="Please use a camera at the Cat tree."};
case 31: return new TrainingNoteEng{Desc="Use tripods to boost."};
case 32: return new TrainingNoteEng{Desc="Congrats! You’ve unlocked a new photo and got 2 cat money."};
case 33: return new TrainingNoteEng{Desc="There are different photos in different maps."};
case 34: return new TrainingNoteEng{Desc="Please use a gacha item at the Cat tree."};
case 35: return new TrainingNoteEng{Desc="Use catnip to boost."};
case 36: return new TrainingNoteEng{Desc="Congrats! You’ve unlocked a new cat. Different gacha items will unlock different body type of cats."};
case 37: return new TrainingNoteEng{Desc="Let's change to the new cat."};
case 38: return new TrainingNoteEng{Desc="Click on cat's profile picture. It will open the interactive window."};
case 39: return new TrainingNoteEng{Desc="Click on the button of next page to switch cats."};
case 40: return new TrainingNoteEng{Desc="Click on the button of select to use new cat in the levels."};
case 41: return new TrainingNoteEng{Desc="You can also purchase cat money at here."};
case 42: return new TrainingNoteEng{Desc="Congrats! You’ve finished all the tutorials，Here is your reward!"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}