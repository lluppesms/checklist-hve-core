import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CheckList } from "../shared/models/CheckList.model";
import { CheckListRepository } from "../shared/services/checklist.repository";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-action-add',
  templateUrl: './action-add.component.html'
})

export class ActionAddComponent implements OnInit, OnDestroy {
  private listId: number;
  private sub: any;

  public selectedList: CheckList;
  private addForm: FormGroup;

  constructor(private fb: FormBuilder, private repo: CheckListRepository, private router: Router, private route: ActivatedRoute) {
    this.createForm();
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.listId = +params['id'];
      this.repo.getList(this.listId).then(result => {
        this.selectedList = result;
      })
    });
  }
  createForm() {
    this.addForm = this.fb.group({
      categoryText: ['Main', Validators.required],
      actionText: ['', Validators.required]
    });
  }

  addAction(): void {
    this.repo.addAction(this.listId, this.addForm.value.actionText, this.addForm.value.categoryText).then(response => {
      if (response.success) {
        this.returnToList();
      } else {
        alert(response.message);
      }
    })
  }
  returnToList(): void {
    this.router.navigate(['/action/list/', this.listId]);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
