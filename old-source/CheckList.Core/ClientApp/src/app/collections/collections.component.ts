import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CheckSet } from "../shared/models/CheckSet.model";
import { CheckListRepository } from "../shared/services/checklist.repository";
import { AuthService } from "../shared/services/auth.service";

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html'
})

export class CollectionsComponent {
  public collectionList: CheckSet[];
  public selectedCollection: CheckSet;
  constructor(private repo: CheckListRepository, private router: Router, private auth: AuthService) { }

  ngOnInit(): void {
    this.repo.getCollections().then(result => {
      this.collectionList = result;
    })
  }

  onSelect(mySet: CheckSet): void {
    this.selectedCollection = mySet;
    this.router.navigate(['/lists', mySet.setId]);
  }
}
