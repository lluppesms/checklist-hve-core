import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { map } from "rxjs/operators";

import { CheckSet } from "../models/CheckSet.model";
import { CheckList } from "../models/CheckList.model";
import { ActionItem } from "../models/ActionItem.model";
import { AuthService } from "../services/auth.service";

const collectionListUrl = "api/Collections/";
const collectionItemUrl = "api/Collection/";

const listsInSetUrl = "api/Lists/";
const listItemUrl = "api/List/";

const actionListUrl = "api/Actions/";
const actionItemUrl = "api/Action/";
const actionCompleteUrl = "api/ActionComplete/";

@Injectable()
export class CheckListRepository {
  private baseUrl: string;

  constructor(private http: HttpClient, private auth: AuthService, private router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getCollections() {
    return this.http.get<CheckSet[]>(this.baseUrl + collectionListUrl,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }
  public getCollection(id: number) {
    return this.http.get<CheckSet>(this.baseUrl + collectionItemUrl + id,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }

  public getListsInCollection(id: number) {
    return this.http.get<CheckList[]>(this.baseUrl + listsInSetUrl + id,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }
  public getList(id: number) {
    return this.http.get<CheckList>(this.baseUrl + listItemUrl + id,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }
  public getListActions(id: number) {
    return this.http.get<ActionItem[]>(this.baseUrl + actionListUrl + id,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }
  public resetList(id: number) {
    let bearerToken = this.auth.getBearerToken();
    return this.http.get<ActionItem[]>(this.baseUrl + actionListUrl + id,
      { headers: { 'Authorization': bearerToken } })
      .pipe(map(result => {
        var actionsList = result;
        console.log("Resetting list of " + actionsList.length + " items...");
        for (var i = 0; i < actionsList.length; i++) {
          var itemId = actionsList[i].actionId;
          var itemComplete = actionsList[i].isComplete;
          if (itemComplete) {
            const data = {
              "ActionId": itemId,
              "IsComplete": false
            };
            this.http.put(
              this.baseUrl + actionCompleteUrl + itemId, data,
              { headers: { 'Authorization': bearerToken } }
            ).pipe(map(response => {
              console.log("PUT ActionComplete Item:", itemId, " to Completed = False; Response: ", response);
            })).toPromise();
          }
          else {
            console.log("Item:", itemId, " is already set to Completed = false");
          }
        }
      })).toPromise();
  }

  public getAction(id: number) {
    return this.http.get<ActionItem>(this.baseUrl + actionItemUrl + id,
      { headers: { 'Authorization': this.auth.getBearerToken() } })
      .pipe(map(result => { return result; })).toPromise();
  }
  public addAction(listId: number, actionText: string, categoryText: string) {
    const data = {
      "listId": listId,
      "categoryText": categoryText,
      "actionText": actionText
    };
    console.log(data);
    return this.http.post(
      this.baseUrl + actionItemUrl, data,
      { headers: { 'Authorization': this.auth.getBearerToken() } }
    ).pipe(map((response: any) => {
      if (response.IsSuccessStatusCode) {
        console.log("Successfully Created Action!");
        return { "success": true, "message": "OK" };
      }
      else {
        console.log("Error Creating Action: ", response);
        return { "success": false, "message": this.getHttpResponseStatusReason(response) };
      }
    })).toPromise();
  }
  public updateActionStatus(item: ActionItem) {
    const data = {
      "actionId": item.actionId,
      "isComplete": item.isComplete
    };
    return this.http.put(
      this.baseUrl + actionCompleteUrl + item.actionId, data,
      { headers: { 'Authorization': this.auth.getBearerToken() } }
    ).pipe(map((response: any) => {
      if (response.IsSuccessStatusCode) {
        console.log("Successfully Updated Action Status Item: ", item.actionId, " Completed = " + item.isComplete, "; Response: ", response);
        return { "success": true, "message": "OK" };
      }
      else {
        console.log("Error Updating Action Status Id ", item.actionId, ": ", response);
        return { "success": false, "message": this.getHttpResponseStatusReason(response) };
      }
    })).toPromise();
  }
  public editAction(listId: number, actionId: number, actionText: string, categoryText: string) {
    const data = {
      "listId": listId,
      "actionId": actionId,
      "categoryText": categoryText,
      "actionText": actionText
    };
    console.log(data);
    return this.http.put(
      this.baseUrl + actionItemUrl, data,
      { headers: { 'Authorization': this.auth.getBearerToken() } }
    ).pipe(map((response: any) => {
      if (response.IsSuccessStatusCode) {
        console.log("Successfully Updated Action Id ", actionId);
        return { "success": true, "message": "OK" };
      }
      else {
        console.log("Error Updating Action Id ", actionId, ": ", response);
        return { "success": false, "message": this.getHttpResponseStatusReason(response) };
      }
    })).toPromise();
  }
  public deleteAction(actionId: number) {
    return this.http.delete(
      this.baseUrl + actionItemUrl + actionId,
      { headers: { 'Authorization': this.auth.getBearerToken() } }
    ).pipe(map((response: any) => {
      if (response.IsSuccessStatusCode) {
        console.log("Successfully Deleted Action Id ", actionId);
        return { "success": true, "message": "OK" };
      }
      else {
        console.log("Error Deleting Action Id ", actionId, ": ", response);
        return { "success": false, "message": this.getHttpResponseStatusReason(response) };
      }
    })).toPromise();
  }

  public groupActions(collection, attribute) {
    var groups = [];
    var sortedActions = collection;
    var groupValue = "_INVALID_GROUP_VALUE_";
    for (var i = 0; i < sortedActions.length; i++) {
      var action = sortedActions[i];
      if (action[attribute] !== groupValue) {
        var group = {
          categoryText: action[attribute],
          actions: []
        };
        groupValue = group.categoryText;
        groups.push(group);
      }
      group.actions.push(action);
    }
    return groups;
  }
  public countCompletedActions(actionList) {
    var counts =
    {
      actionsCount: 0,
      actionsComplete: 0,
      percentComplete: 0
    }
    for (var i = 0; i < actionList.length; i++) {
      counts.actionsCount++;
      if (actionList[i].isComplete) { counts.actionsComplete++; }
    }
    if (counts.actionsCount > 0) {
      counts.percentComplete = counts.actionsComplete / counts.actionsCount;
    }
    return counts;
  }

  public getHttpResponseStatusReason(response) {
    if (response.Headers.length > 0) {
      for (var i = 0; i < response.Headers.length; i++) {
        if (response.Headers[i].Key == "X-Status-Reason") {
          return response.Headers[i].Value;
        }
      }
    }
    return response.StatusCode + " " + response.ReasonPhrase
  }
}
