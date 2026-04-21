import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ActionItem } from "../shared/models/actionitem.model";
import { CheckList } from "../shared/models/CheckList.model";
import { CheckListRepository } from "../shared/services/checklist.repository";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-action-edit',
  templateUrl: './action-edit.component.html'
})

export class ActionEditComponent implements OnInit, OnDestroy {
  private actionId: number;
  private sub: any;

  public selectedAction: ActionItem;
  public selectedList: CheckList;
  private editForm: FormGroup;

  constructor(private fb: FormBuilder, private repo: CheckListRepository, private router: Router, private route: ActivatedRoute) {
    this.createForm();
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.actionId = +params['id']; // (+) converts string 'id' to a number
      this.repo.getAction(this.actionId).then(result => {
        this.selectedAction = result;
        this.editForm.setValue({
          categoryText: this.selectedAction.categoryText,
          actionText: this.selectedAction.actionText,
          actionId: this.selectedAction.actionId,
          listId: this.selectedAction.listId
        });
      })
    });
  }

  createForm() {
    // this.angForm = this.fb.group(ActionItem);
    this.editForm = this.fb.group({
      categoryText: ['Main', Validators.required],
      actionText: ['', Validators.required],
      listId: [0, Validators.required],
      actionId: [0, Validators.required]
    });
  }

  editAction(): void {
    this.repo.editAction(this.selectedAction.listId, this.selectedAction.actionId, this.editForm.value.actionText, this.editForm.value.categoryText)
      .then(response => {
        if (response.success) {
          this.returnToList();
        } else {
          alert(response.message);
        }
      })
  }
  returnToList(): void {
    this.router.navigate(['/action/list/', this.selectedAction.listId]);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
