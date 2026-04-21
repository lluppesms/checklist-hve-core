import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CheckSet } from "../shared/models/CheckSet.model";
import { CheckList } from "../shared/models/CheckList.model";
import { CheckListRepository } from "../shared/services/checklist.repository";

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html'
})

export class ListsComponent {
  private id: number;
  private sub: any;

  public listList: CheckList[];
  public selectedList: CheckList;
  public selectedCollection: CheckSet;

  constructor(private repo: CheckListRepository, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id']; // (+) converts string 'id' to a number
      this.repo.getCollection(this.id).then(result => {
        this.selectedCollection = result;
      })

      this.repo.getListsInCollection(this.id).then(result => {
        this.listList = result;
      })
    });
  }

  onSelect(myList: CheckList): void {
    this.router.navigate(['/action/list/', myList.listId]);
  }

  resetChecks(myList: CheckList) {
    if (myList.actionsComplete > 0) {
      if (confirm("Are you sure you want to reset list " + myList.listName + "?")) {
        console.log("Resetting list " + myList.listId);
        this.repo.resetList(myList.listId).then(result => {
          myList.actionsComplete = 0;
        })
      }
    }
    event.stopPropagation();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
