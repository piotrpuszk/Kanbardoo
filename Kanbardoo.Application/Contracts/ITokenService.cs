﻿using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts;
public interface ICreateToken
{
    string Create(KanUser user);
}
