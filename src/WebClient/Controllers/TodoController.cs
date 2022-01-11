using Domain.Models.Shared;
using Domain.Models.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UseCase.Shared;
using UseCase.Todos.Create;
using UseCase.Todos.Delete;
using UseCase.Todos.Edit;
using UseCase.Todos.Get;
using UseCase.Todos.Search;
using UseCase.Todos.UpdateStatus;
using WebClient.Models.Todos;

namespace WebClient.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TodoCreateUseCase _todoCreateUseCase;
        private readonly ITodoSearchQueryService _todoSearchQueryService;
        private readonly TodoUpdateStatusUseCase _todoUpdateStatusUseCase;
        private readonly TodoGetUseCase _todoGetUseCase;
        private readonly TodoEditUseCase _todoEditUseCase;
        private readonly TodoDeleteUseCase _todoDeleteUseCase;

        public TodoController(
            UserManager<IdentityUser> userManager,
            TodoCreateUseCase todoCreateUseCase,
            ITodoSearchQueryService todoSearchQueryService,
            TodoUpdateStatusUseCase todoUpdateStatusUseCase,
            TodoGetUseCase todoGetUseCase, TodoEditUseCase todoEditUseCase,
            TodoDeleteUseCase todoDeleteUseCase)
        {
            _userManager = userManager;
            _todoCreateUseCase = todoCreateUseCase;
            _todoSearchQueryService = todoSearchQueryService;
            _todoUpdateStatusUseCase = todoUpdateStatusUseCase;
            _todoGetUseCase = todoGetUseCase;
            _todoEditUseCase = todoEditUseCase;
            _todoDeleteUseCase = todoDeleteUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TodoViewModel viewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(User) ?? "";

                #region TODO一覧取得

                var statuses = new List<int>();
                if (viewModel.IsIncompleted)
                {
                    statuses.Add((int)TodoStatus.未完了);
                }
                if (viewModel.IsCompleted)
                {
                    statuses.Add((int)TodoStatus.完了);
                }

                var searchCommand = new TodoSearchCommand(
                    userSession: new UserSession(userId),
                    keyword: viewModel.Keyword,
                    statuses: statuses,
                    pageNum: viewModel.PageNum,
                    pageSize: viewModel.PageSize);
                var searchResult = await _todoSearchQueryService.ExecuteAsync(searchCommand);

                viewModel.Summaries = searchResult.Summaries
                    .Select(x => new TodoSummaryViewModel(x))
                    .ToArray();

                #endregion

                #region 選択されたTODOの詳細を取得

                if (!string.IsNullOrWhiteSpace(viewModel.Id))
                {
                    var getCommand = new TodoGetCommand(
                        userSession: new UserSession(userId),
                        viewModel.Id);
                    var getResult = await _todoGetUseCase.ExecuteAsync(getCommand);

                    viewModel.PostInputModel ??= new TodoPostInputModel();
                    viewModel.TodoGetViewModel ??= new TodoGetViewModel();

                    viewModel.PostInputModel.Title = getResult.Todo.Title;
                    viewModel.PostInputModel.Description = getResult.Todo.Description ?? "";
                    viewModel.TodoGetViewModel.StatusString = getResult.Todo.StatusName;
                    viewModel.TodoGetViewModel.CreatedDateTime = getResult.Todo.CreatedDateTime;
                }

                #endregion
            }
            catch (Exception e)
            {
                if (e is not (DomainException or UseCaseException)) throw;

                viewModel.ErrorMessage = e.Message;
            }

            return View("Index", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(TodoViewModel viewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(User) ?? "";

                if (string.IsNullOrWhiteSpace(viewModel.Id))
                {
                    var createCommand = new TodoCreateCommand(
                        userSession: new UserSession(userId),
                        title: viewModel.PostInputModel?.Title ?? "",
                        description: viewModel.PostInputModel?.Description);
                    var createResult = await _todoCreateUseCase.ExecuteAsync(createCommand);

                    viewModel.Id = createResult.Id;
                }
                else
                {
                    var editCommand = new TodoEditCommand(
                        userSession: new UserSession(userId),
                        id: viewModel.Id,
                        title: viewModel.PostInputModel?.Title ?? "",
                        description: viewModel.PostInputModel?.Description);
                    await _todoEditUseCase.ExecuteAsync(editCommand);
                }
            }
            catch (Exception e)
            {
                if (e is not (DomainException or UseCaseException)) throw;

                viewModel.ErrorMessage = e.Message;
            }

            viewModel.PostInputModel = null;
            viewModel.TodoGetViewModel = null;
            viewModel.CompleteInputModel = null;

            return RedirectToAction("Index", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(TodoViewModel viewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var command = new TodoDeleteCommand(
                    userSession: new UserSession(userId),
                    id: viewModel.Id);
                await _todoDeleteUseCase.ExecuteAsync(command);

                viewModel.Id = "";
                viewModel.TodoGetViewModel = null;
                viewModel.PostInputModel = null;
            }
            catch (Exception e)
            {
                if (e is not (DomainException or UseCaseException)) throw;

                viewModel.ErrorMessage = e.Message;
            }

            return RedirectToAction("Index", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusAsync(TodoViewModel viewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(User) ?? "";

                var command = new TodoUpdateStatusCommand(
                    userSession: new UserSession(userId),
                    id: viewModel.CompleteInputModel?.TodoId ?? "",
                    status: viewModel.CompleteInputModel?.Status ?? default);
                await _todoUpdateStatusUseCase.ExecuteAsync(command);
            }
            catch (Exception e)
            {
                if (e is not (DomainException or UseCaseException)) throw;

                viewModel.ErrorMessage = e.Message;
            }

            viewModel.PostInputModel = null;
            viewModel.CompleteInputModel = null;
            viewModel.TodoGetViewModel = null;

            return RedirectToAction("Index", viewModel);
        }
    }
}
