import { Component, Inject, OnInit, OnDestroy, PACKAGE_ROOT_URL } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ActionItem } from "../shared/models/actionitem.model";
import { CheckList } from "../shared/models/CheckList.model";
import { CheckListRepository } from "../shared/services/checklist.repository";

@Component({
  selector: 'app-action-list',
  templateUrl: './action-list.component.html'
})

export class ActionsComponent implements OnInit, OnDestroy {
  private id: number;
  private sub: any;

  public actionsCount: number;
  public actionsComplete: number;
  public percentComplete: number;

  public actionsList: ActionItem[];
  public selectedAction: ActionItem;
  public selectedList: CheckList;
  public groupedActions: ActionItem[];

  constructor(private repo: CheckListRepository, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id']; // (+) converts string 'id' to a number
      this.repo.getList(this.id).then(result => {
        this.selectedList = result;
      })
      this.getItems(this.id);
    });
  }

  getItems(listId: number) {
    this.repo.getListActions(listId).then(result => {
      this.actionsList = result;
      this.groupedActions = this.repo.groupActions(this.actionsList, 'categoryText');
      this.countCompletedItems();
    })
  }

  addAction(): void {
    this.router.navigate(['/action/add/', this.selectedList.listId]);
  }

  editAction(actionId) {
    this.router.navigate(['/action/edit/', actionId]);
    event.stopPropagation();
  }

  deleteAction(actionId, actionText) {
    if (confirm("Are you sure you want to delete this item?  \n" + actionText)) {
      this.repo.deleteAction(actionId).then(response => {
        if (response.success) {
          this.getItems(this.id);
        } else {
          alert(response.message);
        }
      })
    }
    event.stopPropagation();
  }

  onSelect(item: ActionItem): void {
    item.isComplete = !item.isComplete;
    this.selectedAction = item;
    this.repo.updateActionStatus(item).then(response => {
      if (response.success) {
        this.countCompletedItems();
      } else {
        alert(response.message);
      }
    })
  }

  countCompletedItems() {
    var counts = this.repo.countCompletedActions(this.actionsList);
    this.actionsCount = counts.actionsCount;
    this.actionsComplete = counts.actionsComplete;
    this.percentComplete = counts.percentComplete;
  }

  resetChecks() {
    if (this.actionsComplete > 0) {
      if (confirm("Are you sure you want to reset list " + this.selectedList.listName + "?")) {
        console.log("Resetting list " + this.selectedList.listId);
        for (var i = 0; i < this.actionsList.length; i++) {
          if (this.actionsList[i].isComplete) {
            var item = this.actionsList[i];
            this.actionsList[i].isComplete = false;
            item.isComplete = false;
            this.repo.updateActionStatus(item).then(result => {
              this.countCompletedItems();
            })
          }
        }
        this.actionsComplete = 0;
        this.percentComplete = 0;
      }
    }
    event.stopPropagation();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
