import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { MessagesComponent } from "./messages/messages.component";
import { MemberListComponent } from "./members/member-list/member-list.component";
import { ListsComponent } from "./lists/lists.component";
import { AuthGuard } from "./guards/auth.guard";
import { MemberDetailComponent } from "./members/member-detail/member-detail.component";
import { MemberDetailResolver } from "./resolvers/member-detail.resolver";
import { MemberListResolver } from "./resolvers/member-list.resolver";
import { MemberEditComponent } from "./members/member-edit/member-edit.component";
import { MemberEditResolver } from "./resolvers/member-edit.resolver";
import { PreventUnsavedChanges } from "./guards/prevent-unsaved-changes.guard";
import { ListsResolver } from "./resolvers/lists.resolver";
import { MessagesResolver } from "./resolvers/messages.resolver";

export const appRoots: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'members',
        component: MemberListComponent, resolve: { users: MemberListResolver }
      },
      {
        path: 'members/:id',
        component: MemberDetailComponent, resolve: { user: MemberDetailResolver }
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        resolve: { user: MemberEditResolver },
        canDeactivate: [PreventUnsavedChanges]
      },
      {
        path: 'messages',
        component: MessagesComponent,
        resolve: { messages: MessagesResolver }
      },
      {
        path: 'lists',
        component: ListsComponent,
        resolve: { user: ListsResolver}
      }
    ]
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
]
