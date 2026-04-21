import { NgModule } from '@angular/core';
import { CheckListRepository } from "../services/checklist.repository";
import { AuthService } from "../services/auth.service";

import { CheckSet } from "../models/CheckSet.model";
import { CheckList } from "../models/CheckList.model";
import { ActionItem } from "../models/ActionItem.model";

@NgModule({
  providers: [CheckListRepository, AuthService, CheckSet, CheckList, ActionItem]
})
export class ModelModule { }
