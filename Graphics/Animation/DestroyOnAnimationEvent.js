#pragma strict

var toDestroy  : GameObject;

function DestroyOnAnimationEvent () {
	Destroy(toDestroy);
}